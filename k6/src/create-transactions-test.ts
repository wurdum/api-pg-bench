import { check } from 'k6';
import { Options } from 'k6/options';
import http from 'k6/http';

const baseUrl = __ENV.API_HOSTNAME || 'http://localhost:5001';

export let options: Options = {
  insecureSkipTLSVerify: true,
  summaryTrendStats: ['avg', 'min', 'med', 'max', 'p(95)', 'p(99)', 'count'],
  scenarios: {
    transactions: {
      executor: 'ramping-arrival-rate',
      preAllocatedVUs: 100,
      stages: [
        { target: 10000, duration: '10s' },
        { target: 30000, duration: '20s' },
        { target: 10000, duration: '10s' }
      ]
    }
  }
};

const currencies = ['UAH', 'USD', 'EUR'];

function randomString(length: number) {
  return Math.round((Math.pow(36, length + 1) - Math.random() * Math.pow(36, length))).toString(36).slice(1);
}

function randomNumber(min: number, max: number) {
  return Math.floor(Math.random() * (max - min + 1) + min);
}

function randomDate() {
  const start = new Date(2022, 1, 24);
  const end = new Date();
  return new Date(start.getTime() + Math.random() * (end.getTime() - start.getTime()));
}

export function setup() {
  // Ensure that we have at least 100 families.
  const familiesCountResponse = http.get(`${baseUrl}/counts/families`);
  var familiesCount = familiesCountResponse.json('value');
  if (typeof familiesCount !== 'number') {
    throw 'Unable to query families count';
  }

  if (familiesCount < 100) {
    const familiesToAdd = 100 - familiesCount;
    console.log(`Adding ${familiesToAdd} families...`);

    for (let i = 0; i < familiesToAdd; i++) {
      http.post(`${baseUrl}/families`, JSON.stringify({
        name: randomString(30)
      }), {
        headers: { 'Content-Type': 'application/json' }
      });
    }

    familiesCount = 100;
    console.log(`Added ${familiesToAdd} families`);
  }

  // Ensure that we have at least 1000 accounts.
  const accountsCountResponse = http.get(`${baseUrl}/counts/accounts`);
  var accountsCount = accountsCountResponse.json('value');
  if (typeof accountsCount !== 'number') {
    throw 'Unable to query accounts count';
  }

  if (accountsCount < 1000) {
    const accountsToAdd = 1000 - accountsCount;
    console.log(`Adding ${accountsToAdd} accounts...`);

    for (let i = 0; i < accountsToAdd; i++) {
      http.post(`${baseUrl}/accounts`, JSON.stringify({
        familyId: randomNumber(1, familiesCount),
        name: randomString(30),
        currency: currencies[randomNumber(0, currencies.length - 1)],
        accountGroup: 0
      }), {
        headers: { 'Content-Type': 'application/json' }
      });
    }

    accountsCount = 1000;
    console.log(`Added ${accountsToAdd} accounts`);
  }

  console.log(`Found ${familiesCount} families and ${accountsCount} accounts`);

  return {
    familiesCount: familiesCount,
    accountsCount: accountsCount
  };
}

export default function(data: { accountsCount: number }) {
  const payload = JSON.stringify({
    accountId: randomNumber(1, data.accountsCount),
    description: randomString(60),
    category: randomString(30),
    amount: 100.1,
    createdAt: randomDate().toISOString()
  });

  const params = {
    headers: {
      'Content-Type': 'application/json',
    },
  };

  const response = http.post(`${baseUrl}/transactions`, payload, params);

  check(response, {
    [`transaction created successfully`]: () => response.status == 201
  });
}
