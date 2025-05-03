

<div class="container mt-5">
        <div class="row">
            <div class="col-md-4">
                <div class="card text-center">
                    <div class="card-body">
                        <img src="https://via.placeholder.com/150" class="rounded-circle mb-3" alt="Profile Picture">
                        <h5 class="card-title text-highlight">John Doe</h5>
                        <p class="card-text">Account Number: 123456789</p>
                        <p class="card-text">Balance: <span class="text-highlight">$10,000</span></p>
                        <button class="btn btn-custom">Edit Profile</button>
                    </div>
                </div>
            </div>
            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title text-highlight">Recent Transactions</h5>
                        <table class="table table-dark table-hover">
                            <thead>
                                <tr>
                                    <th>Date</th>
                                    <th>Description</th>
                                    <th>Amount</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>2023-03-01</td>
                                    <td>ATM Withdrawal</td>
                                    <td class="text-danger">- $200</td>
                                </tr>
                                <tr>
                                    <td>2023-02-28</td>
                                    <td>Salary Deposit</td>
                                    <td class="text-success">+ $5,000</td>
                                </tr>
                                <tr>
                                    <td>2023-02-25</td>
                                    <td>Online Purchase</td>
                                    <td class="text-danger">- $150</td>
                                </tr>
                            </tbody>
                        </table>
                        <button class="btn btn-custom">View All Transactions</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="container mt-4">
    <div class="row">
        <div class="col-md-12 text-center">
            <h5 class="text-highlight">Actions</h5>
            <button class="btn btn-custom m-2" onclick="showForm('depositForm')">Пополнить счет</button>
            <button class="btn btn-custom m-2" onclick="showForm('withdrawForm')">Снять со счета</button>
            <button class="btn btn-custom m-2" onclick="showForm('transferForm')">Перевести на другой счет</button>
            <button class="btn btn-custom m-2" onclick="showForm('viewAccountsForm')">Просмотр моих счетов</button>
        </div>
    </div>
    <div class="row mt-4">
        <div class="col-md-12">
            <div id="depositForm" class="action-form" style="display: none;">
                <h5>Пополнить счет</h5>
                <form>
                    <div class="form-group">
                        <label for="depositAmount">Сумма</label>
                        <input type="number" class="form-control" id="depositAmount" placeholder="Введите сумму">
                    </div>
                    <button type="submit" class="btn btn-custom">Пополнить</button>
                </form>
            </div>
            <div id="withdrawForm" class="action-form" style="display: none;">
                <h5>Снять со счета</h5>
                <form>
                    <div class="form-group">
                        <label for="withdrawAmount">Сумма</label>
                        <input type="number" class="form-control" id="withdrawAmount" placeholder="Введите сумму">
                    </div>
                    <button type="submit" class="btn btn-custom">Снять</button>
                </form>
            </div>
            <div id="transferForm" class="action-form" style="display: none;">
                <h5>Перевести на другой счет</h5>
                <form>
                    <div class="form-group">
                        <label for="transferAccount">Номер счета</label>
                        <input type="text" class="form-control" id="transferAccount" placeholder="Введите номер счета">
                    </div>
                    <div class="form-group">
                        <label for="transferAmount">Сумма</label>
                        <input type="number" class="form-control" id="transferAmount" placeholder="Введите сумму">
                    </div>
                    <button type="submit" class="btn btn-custom">Перевести</button>
                </form>
            </div>
            <div id="viewAccountsForm" class="action-form" style="display: none;">
                <h5>Просмотр моих счетов</h5>
                <p>Функция "Просмотр моих счетов" пока не реализована.</p>
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