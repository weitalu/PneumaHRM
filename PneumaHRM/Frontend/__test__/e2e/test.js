require('chromedriver');
const webdriver = require('selenium-webdriver');
const { By, until } = webdriver;

const chai = require('chai');
const chaiAsPromised = require('chai-as-promised');

chai.use(chaiAsPromised);
let pref = new webdriver.logging.Preferences();
pref.setLevel('browser', webdriver.logging.Level.ALL);
let driver = new webdriver.Builder().forBrowser('chrome').setLoggingPrefs(pref).build();
const expect = chai.expect;

describe('PneumasoftHRM End to End Test Suite', done => {
    it('has dashboard', done => {
        console.log('render dashboard');
        driver.get('https://localhost:44364/dashboard')
            .then(() => driver.wait(until.elementLocated({ id: "Dashboard" })))
            .then(() => done())
    });

    it('has leave balance', done => {
        console.log('render leave balance');
        driver.get('https://localhost:44364/balance')
            .then(() => driver.wait(until.elementLocated({ id: "LeaveBalance" })))
            .then(() => done());
    })
    it('has leave request', done => {
        console.log('render leave request');
        driver.get('https://localhost:44364/leaverequest')
            .then(() => driver.wait(until.elementLocated({ id: "LeaveRequest" })))
            .then(() => done());
    })
});