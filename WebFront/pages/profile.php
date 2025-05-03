<?php

if (!isset($_SESSION['user_logged_in']) || !$_SESSION['user_logged_in']) {
    echo "<script>window.location.href = '/login.php';</script>";
    exit();
}
?>
<div class="container mt-5">
        <div class="row">
            <div class="col-md-4">
                <div class="card text-center">
                    <div class="card-body">
                        <img src="assets/images/images.png" class="rounded-circle mb-3" alt="Фото профиля">
                        <h5 class="card-title text-highlight">Иван Иванов</h5>
                        <p class="card-text">Номер счета: 123456789</p>
                        <p class="card-text">Электронная почта: IvanIvanov@example.com</p>
                        <p class="card-text">Баланс: <span class="text-highlight">10,000 ₽</span></p>
                        <button class="btn btn-custom">Редактировать профиль</button>
                    </div>
                </div>
            </div>
            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title text-highlight">Последние транзакции</h5>
                        <table class="table table-dark table-hover">
                            <thead>
                                <tr>
                                    <th>Дата</th>
                                    <th>Описание</th>
                                    <th>Сумма</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>2023-03-01</td>
                                    <td>Снятие в банкомате</td>
                                    <td class="text-danger">- 200 ₽</td>
                                </tr>
                                <tr>
                                    <td>2023-02-28</td>
                                    <td>Зачисление зарплаты</td>
                                    <td class="text-success">+ 5,000 ₽</td>
                                </tr>
                                <tr>
                                    <td>2023-02-25</td>
                                    <td>Покупка онлайн</td>
                                    <td class="text-danger">- 150 ₽</td>
                                </tr>
                            </tbody>
                        </table>
                        <button class="btn btn-custom">Просмотреть все транзакции</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="container mt-4">
    <div class="row">
        <div class="col-md-12 text-center">
            <h5 class="text-highlight mb-4">Операции над вашими счетами</h5>
            <div class="btn-group" role="group">
                <button class="btn btn-warning m-2" onclick="showForm('depositForm')">Пополнить счет</button>
                <button class="btn btn-warning m-2" onclick="showForm('withdrawForm')">Снять со счета</button>
                <button class="btn btn-warning m-2" onclick="showForm('transferForm')">Перевести на другой счет</button>
                <button class="btn btn-warning m-2" onclick="showForm('viewAccountsForm')">Просмотр моих счетов</button>
            </div>
        </div>
    </div>

    <div class="row mt-4">
        <div class="col-md-12">
            <!-- Форма пополнения -->
            <div id="depositForm" class="action-form card p-4 my-5" style="display: none;">
                <h5 class="mb-3">Пополнить счет</h5>
                <form>
                    <div class="form-group">
                        <label for="phoneNumberDeposit">Номер телефона</label>
                        <input type="text" class="form-control" id="phoneNumberDeposit" placeholder="Введите номер телефона">
                    </div>
                    <div class="form-group">
                        <label for="depositAmount">Сумма</label>
                        <input type="number" class="form-control" id="depositAmount" placeholder="Введите сумму">
                    </div>
                    <button type="submit" class="btn btn-warning mt-3">Пополнить</button>
                </form>
            </div>

            <!-- Форма снятия -->
            <div id="withdrawForm" class="action-form card p-4 my-5" style="display: none;">
                <h5 class="mb-3">Снять со счета</h5>
                <form>
                    <div class="form-group">
                        <label for="phoneNumberWithdraw">Номер телефона</label>
                        <input type="text" class="form-control" id="phoneNumberWithdraw" placeholder="Введите номер телефона">
                    </div>
                    <div class="form-group">
                        <label for="withdrawAmount">Сумма</label>
                        <input type="number" class="form-control" id="withdrawAmount" placeholder="Введите сумму">
                    </div>
                    <button type="submit" class="btn btn-warning mt-3">Снять</button>
                </form>
            </div>

            <!-- Форма перевода -->
            <div id="transferForm" class="action-form card p-4 my-5" style="display: none;">
                <h5 class="mb-3">Перевести на другой счет</h5>
                <form>
                    <div class="form-group">
                        <label for="phoneNumberTransfer">Номер телефона</label>
                        <input type="text" class="form-control" id="phoneNumberTransfer" placeholder="Введите номер телефона">
                    </div>
                    <div class="form-group">
                        <label for="transferAmount">Сумма</label>
                        <input type="number" class="form-control" id="transferAmount" placeholder="Введите сумму">
                    </div>
                    <button type="submit" class="btn btn-warning mt-3">Перевести</button>
                </form>
            </div>

            <!-- Форма просмотра счетов -->
            <div id="viewAccountsForm" class="action-form card p-4 my-5" style="display: none;">
                <h5 class="mb-3">Просмотр моих счетов</h5>
                <table class="table table-dark table-hover">
                    <thead>
                        <tr>
                            <th>Номер вашего счета</th>
                            <th>Тип счета</th>
                            <th>Баланс</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>+1234567890</td>
                            <td>Сберегательный</td>
                            <td>10,000 ₽</td>
                        </tr>
                        <tr>
                            <td>+0987654321</td>
                            <td>Текущий</td>
                            <td>5,000 ₽</td>
                        </tr>
                        <tr>
                            <td>+4567891230</td>
                            <td>Депозитный</td>
                            <td>20,000 ₽</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>

<script>
    function showForm(formId) {
        const forms = document.querySelectorAll('.action-form');
        forms.forEach(form => form.style.display = 'none');
        document.getElementById(formId).style.display = 'block';
    }
</script>
