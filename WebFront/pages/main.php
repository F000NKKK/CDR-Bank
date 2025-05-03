<div class="container my-5">
    <div class="row text-center">
        <h2 class="mb-4">Особенности CDR Bank</h2>
        <div class="col-md-4">
            <div class="card shadow-sm">
                <div class="card-body">
                    <h5 class="card-title">Надежность</h5>
                    <p class="card-text">Ваши средства защищены благодаря современным технологиям безопасности.</p>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card shadow-sm">
                <div class="card-body">
                    <h5 class="card-title">Удобство</h5>
                    <p class="card-text">Мобильное приложение и интернет-банк доступны 24/7.</p>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card shadow-sm">
                <div class="card-body">
                    <h5 class="card-title">Выгодные условия</h5>
                    <p class="card-text">Низкие проценты по кредитам и высокие ставки по вкладам.</p>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="container my-5">
    <div class="row text-center">
        <h2 class="mb-4">Кредитный калькулятор</h2>
        <div class="col-md-6 offset-md-3">
            <div class="card shadow-sm">
                <div class="card-body">
                    <form id="loanCalculator">
                        <div class="mb-3">
                            <label for="loanAmount" class="form-label">Сумма кредита (₽)</label>
                            <input type="number" class="form-control" id="loanAmount" placeholder="Введите сумму" required>
                        </div>
                        <div class="mb-3">
                            <label for="interestRate" class="form-label">Процентная ставка (%)</label>
                            <input type="number" class="form-control" id="interestRate" placeholder="Введите ставку" required>
                        </div>
                        <div class="mb-3">
                            <label for="loanTerm" class="form-label">Срок кредита (в месяцах)</label>
                            <input type="number" class="form-control" id="loanTerm" placeholder="Введите срок" required>
                        </div>
                        <button type="button" class="btn btn-warning w-100" onclick="calculateLoan()">Рассчитать</button>
                    </form>
                    <div class="mt-4" id="loanResult" style="display: none;">
                        <h5>Результаты расчета:</h5>
                        <p>Ежемесячный платеж: <span id="monthlyPayment"></span> ₽</p>
                        <p>Общая сумма выплат: <span id="totalPayment"></span> ₽</p>
                        <p>Общая сумма процентов: <span id="totalInterest"></span> ₽</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<script>
    function calculateLoan() {
        const loanAmount = parseFloat(document.getElementById('loanAmount').value);
        const interestRate = parseFloat(document.getElementById('interestRate').value) / 100 / 12;
        const loanTerm = parseInt(document.getElementById('loanTerm').value);

        if (isNaN(loanAmount) || isNaN(interestRate) || isNaN(loanTerm) || loanAmount <= 0 || interestRate <= 0 || loanTerm <= 0) {
            alert('Пожалуйста, введите корректные значения.');
            return;
        }

        const monthlyPayment = (loanAmount * interestRate) / (1 - Math.pow(1 + interestRate, -loanTerm));
        const totalPayment = monthlyPayment * loanTerm;
        const totalInterest = totalPayment - loanAmount;

        document.getElementById('monthlyPayment').textContent = monthlyPayment.toFixed(2);
        document.getElementById('totalPayment').textContent = totalPayment.toFixed(2);
        document.getElementById('totalInterest').textContent = totalInterest.toFixed(2);
        document.getElementById('loanResult').style.display = 'block';
    }
</script>