# Миграции баз данных.
Для добавления миграций баз данных используется команда .\CiCd\Db\Add-And-Update-Migrations.ps1 -MigrationName "{НАЗВАНИЕ МИГРАЦИИ}". Данный скрипт выполняет миграции для всех баз данных, вызывать его нужно в корне репозиторияю
Для применения миграций баз данных используется команда .\CiCd\Db\Update-Migrations.ps1
# End Points


## Пополнение: /banking/replenish
### Заголовки:
* bearerToken: SALKJlkjfsakljflkjasASf8u32kalsf
### Тело запроса:
```json
{
    "bankingAccount": "446a2b43-bc7c-41bd-95c4-06bea3e7fd16",
    "amount": 100
}
```



## Перевод другому клиенту: /banking/transfer
### Заголовки:
* bearerToken:  SALKJlkjfsakljflkjasASf8u32kalsf
### Тело запроса:
```json
{
    "bankingAccount": "446a2b43-bc7c-41bd-95c4-06bea3e7fd16",
    "recipientTelephoneNumber": 89994528712,
    "amount": 100
}
```
## Перевод со счёта на счёт /banking/bank-account/transfer
### Заголовки:
* bearerToken:  SALKJlkjfsakljflkjasASf8u32kalsf
### Тело запроса:
```json
{
  "bankingAccount": "446a2b43-bc7c-41bd-95c4-06bea3e7fd16"
}
```
## Снятие: /banking/withdraw
### Заголовки:
* bearerToken: SALKJlkjfsakljflkjasASf8u32kalsf
### Тело запроса:
```json
{
  "bankingAccount": "446a2b43-bc7c-41bd-95c4-06bea3e7fd16",
  "amount": 100
}
```

## Открытие дебетового счёта /banking/bank-account/open/debit
### Заголовки:
* bearerToken:  SALKJlkjfsakljflkjasASf8u32kalsf
### Тело запроса:
```json
{
  "name": "",
  "type": ""
}
```

## Открытие кредетового счёта /banking/bank-account/open/credit
### Заголовки:
* bearerToken:  SALKJlkjfsakljflkjasASf8u32kalsf
### Тело запроса:
```json
{
  "name": "",
  "type": "",
  "limit": 20000
}
```
## Закрытие счёта /banking/bank-account/close
### Заголовки:
* bearerToken:  SALKJlkjfsakljflkjasASf8u32kalsf
### Тело запроса:
```json
{
  "bankingAccount": "446a2b43-bc7c-41bd-95c4-06bea3e7fd16"
}
```

## Редактирование счёта /banking/bank-account/edit
### Заголовки:
* bearerToken:  SALKJlkjfsakljflkjasASf8u32kalsf
### Тело запроса:
```json
{
  "{имя параметра}": "",
  ...
}
```

## Получение данных по счётам /banking/bank-account/get-data
### Заголовки:
* bearerToken:  SALKJlkjfsakljflkjasASf8u32kalsf
### Тело запроса:
```json
{
  "bankingAccount": "446a2b43-bc7c-41bd-95c4-06bea3e7fd16"
}
```


## Регистрация /account/registration
### Тело запроса:
```json
{
  "recipientTelephoneNumber": 89994528712,
  "password": "(в base64 или хэш предпочтительней хэш)"
}
```

## Получение токена /account/get-token без тела запроса
### Заголовки:
* recipientTelephoneNumber: 89994528712,
* password: 1234


## Получение данных по аккаунту клиента /account/get-user
### Заголовки:
* bearerToken:  SALKJlkjfsakljflkjasASf8u32kalsf


## Получение данных по счётам /account/edit
### Заголовки:
* bearerToken:  SALKJlkjfsakljflkjasASf8u32kalsf
### Тело запроса:
```json
{
  "{имя параметра}": "",
  ...
}
```

## Получение данных по счётам /account/change/password
### Заголовки:
* bearerToken:  SALKJlkjfsakljflkjasASf8u32kalsf
### Тело запроса:
```json
{
  "oldPassword": "",
  "newPassword": ""
}
```