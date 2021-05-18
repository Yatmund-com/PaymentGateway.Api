# PaymentGateway.Api
Checkout.com technical test

## Description

A Payment Gateway Api with two endpoints. One to process a payment for a merchant, and another to retrieve details of a previously made payment.

## How does it work?

Process Payment endpoint accepts a POST request with card details, amount, ISO currency code and a merchant Id.

POST Request Curl example :

```
curl -X 'POST' \
      'https://localhost:44367/payment/process' \
      -H 'accept: text/plain' \
      -H 'Content-Type: application/json' \
      -d '{
      "cardNo": "4539 6142 9546 8722",
      "expiry": "05/23",
      "cvv": 312,
      "amount": 999.12,
      "currencyCode": "AUD",
      "merchantId": "merchantId5"
    }
```

Example Response :

```json
{
  "paymentId": "e080bfc0-6c9f-4171-93d7-c9d7e78d52bd",
  "success": true,
  "errorMessage": null,
  "message": null
}
```

Retrieve Payment details endpoint is a GET request that accepts a merchant id and payment Id as query parameters.

Example Get request :

    curl -X 'GET' \
      'https://localhost:44367/payment/details?merchantId=merchantId1' \
      -H 'accept: text/plain'

Example Response :

```json
{
  "maskedCardNo": "***************8807",
  "expiry": "05/23",
  "amount": 12,
  "currencyCode": "GBP",
  "bankMessage": "Payment Processed",
  "success": true,
  "errorMessage": null,
  "message": null
}
```

## For testing

When starting up the api locally, it should load Swagger docs automatically. There you can process payments and also retrieve payments.

Alternatively, you can export the swagger json and you should be able to import it into postman.  (Link at the top of swagger page)

## Assumptions

I don't know enough about Payment Gateways/Acquirers and how they work under the hood, as such there's a few assumptions I've made :

1. Merchant is responsible for validating credit card numbers therefore there is no credit card validation in this api.
2. Acquirer assumes amount is in the card issuers currency, i.e the Payment Gateway has done the currency conversion necessary. However, for this project, I haven't setup currency conversion to keep solution small. If I was going to do it, I would just setup a small/basic integration with a free service like : https://exchangeratesapi.io/

## Notes about Solution

1. Theres some very basic logging using ILogger
2. Basic perfomance metrics can be done using Azure Application Insights
3. For bank integration, I've used https://mocky.io to call out to simulate the bank.
4. To simulate a failed purchase, use a credit card number starting with 5
5. For a database, the api just creates json files on disk. There's a database folder with some files in there already, but also anytime you run the integration tests or process a payment through postman/swagger it should add a new payment log to the Database folder. There's a private method "GetFullDatabasePath()", it's quite ugly in order to also be able to be used for integration tests. Normally an api like this would have a connection to SQL or some other database service and thus you can do proper integration tests.
6. There's some basic CI using Github Actions. 
