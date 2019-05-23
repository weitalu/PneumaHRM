
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PneumaHRM.Models;

namespace PneumaHRM.Controllers
{
    [Route("Api/[controller]")]
    public class GraphQLController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] GraphQLQuery query,
            [FromServices] HrmDbContext db,
            [FromServices] List<DateTime> holidays)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                UserContext = new HrmContext(db, User, holidays),
                Schema = _schema,
                OperationName = query.OperationName,
                Query = query.Query,
                Inputs = inputs,
                ExposeExceptions = true,
            };

            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            db.SaveChanges();

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        public class GraphQLQuery
        {
            public string OperationName { get; set; }
            public string NamedQuery { get; set; }
            public string Query { get; set; }
            public JObject Variables { get; set; } //https://github.com/graphql-dotnet/graphql-dotnet/issues/389
        }
    }
}
