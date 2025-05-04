<?php

if (!isset($_SESSION['user_logged_in']) || !$_SESSION['user_logged_in']) {
    $_SESSION['page'] = "login";
    echo "<script>window.location.href = '/';</script>";
    exit();
}
if (isset($_COOKIE['token'])) {
    $user = getUserProfile($_COOKIE['token']);
    $accounts = getAccounts($_COOKIE['token']);
    $mainAccount = null;
    foreach ($accounts as $account) {
        if (isset($account['isMain']) && $account['isMain'] === true) {
            $mainAccount = $account;
            break;
        }
    }
    $balance = getBalance($_COOKIE['token']);
    $transactions = $mainAccount ? getAccountTransactions($mainAccount['id'], $_COOKIE['token']) : [];
} else {
    $_SESSION['page'] = "login";
    echo "<script>window.location.href = '/';</script>";
    exit();
}

if(!$user) {
    echo "<script>alert('Ошибка: {Error: API cant work!}');</script>";
}
// var_dump($user);
?>
<div class="container mt-5">
    <div class="text-center mb-4">
        <div class="row">
            <div class="col-md-4">
                <div class="card text-center">
                    <div class="card-body">
                        <img src="assets/images/images.png" class="rounded-circle mb-3" alt="Фото профиля">
                        <h5 class="card-title text-highlight"><?php echo isset($user['firstName']) ? htmlspecialchars($user['firstName']) : 'fName'; echo " "; echo isset($user['lastName']) ? htmlspecialchars($user['lastName']) : 'lName'; ?></h5>
                        <p class="card-text">Телефон: <?php echo isset($user['phoneNumber']) ? htmlspecialchars($user['phoneNumber']) : '+9-999-999-99-99';?></p>
                        <p class="card-text">Электронная почта: <?php echo isset($user['email']) ? htmlspecialchars($user['email']) : 'Не указана'; ?></p>
                        <p class="card-text">Баланс: <span class="text-highlight"><?php echo isset($balance) ? htmlspecialchars($balance) : '0'; ?> ₽</span></p>
                        <button class="btn btn-warning">Редактировать профиль</button>
                    </div>
                </div>
            </div>
            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title text-highlight">Последние транзакции основного счета</h5>
                        <table class="table table-dark table-hover">
                            <thead>
                                <tr>
                                    <th>Дата</th>
                                    <th>Описание</th>
                                    <th>Сумма</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php
                                if (isset($transactions) && is_array($transactions) && count($transactions) > 0) {
                                    foreach ($transactions as $transaction) {
                                        echo "<tr>";
                                        echo "<td>" . htmlspecialchars(date("d.m.Y H:i", strtotime($transaction['createdAt']))) . "</td>";
                                        echo "<td>" . htmlspecialchars($transaction['description']) . "</td>";
                                        $transactionType = isset($transaction['type']) ? (int)$transaction['type'] : -1;
                                        $typeLabel = '';
                                        switch ($transactionType) {
                                            case 0:
                                                $typeLabel = "Пополнение";
                                                break;
                                            case 1:
                                                $typeLabel = "Снятие";
                                                break;
                                            case 2:
                                                $typeLabel = "Перевод";
                                                break;
                                            case 3:
                                                $typeLabel = "Внутренний перевод";
                                                break;
                                            default:
                                                $typeLabel = "Неизвестный тип";
                                                break;
                                        }
                                        $amount = $transaction['amount'];
                                        $isDebit = in_array($transactionType, [1, 2]); // Снятие или Перевод
                                        $formattedAmount = ($isDebit ? "- " : "+ ") . abs($amount) . " ₽ ($typeLabel)";
                                        $amountClass = $isDebit ? "text-danger" : "text-success";
                                        echo "<td class='" . $amountClass . "'>" . $formattedAmount . "</td>";
                                        echo "</tr>";
                                    }
                                } else {
                                    echo "<tr><td colspan='3'>Нет доступных транзакций</td></tr>";
                                }
                                ?>
                            </tbody>
                        </table>
                        <!-- <button class="btn btn-warning">Просмотреть все транзакции</button> -->
                    </div>
                </div>
                <!-- Форма создания нового счета -->
                <div id="createAccountForm" class="card p-4 my-5">
                    <h5 class="mb-3">Создать новый счет</h5>
                    <form method="post" action="">
                        <div class="form-group">
                            <label for="accountName">Название счета</label>
                            <input type="text" class="form-control" id="accountName" name="accountName" placeholder="Введите название счета" required>
                        </div>
                        <div class="form-group">
                            <label for="accountType">Тип счета</label>
                            <select class="form-control" id="accountType" name="accountType" required>
                                <option value="0">Дебетовый</option>
                                <option value="1">Кредитовый</option>
                                <option value="2">Вклад</option>
                                <option value="3">Инвестиции</option>
                            </select>
                        </div>
                        <div class="form-group">
                            <label for="creditLimit">Кредитный лимит</label>
                            <input type="number" class="form-control" id="creditLimit" name="creditLimit" placeholder="Введите кредитный лимит (если применимо)">
                        </div>
                        <div class="form-group">
                            <label for="isMain">Сделать основным счетом</label>
                            <select class="form-control" id="isMain" name="isMain" required>
                                <option value="1">Да</option>
                                <option value="0">Нет</option>
                            </select>
                        </div>
                        <button type="submit" class="btn btn-warning mt-3" name="createAccount">Создать счет</button>
                    </form>
                    <?php
                    if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['createAccount'])) {
                        $accountData = [
                            'name' => $_POST['accountName'] ?? '',
                            'type' => isset($_POST['accountType']) ? (int)$_POST['accountType'] : 0,
                            'creditLimit' => isset($_POST['creditLimit']) ? (int)$_POST['creditLimit'] : 0,
                            'isMain' => isset($_POST['isMain']) ? (bool)$_POST['isMain'] : false
                        ];

                        $response = createAccount($accountData, $_COOKIE['token']);

                        if ($response) {
                            echo '<div class="alert alert-success mt-3">Счет успешно создан!</div>';
                        } else {
                            echo '<div class="alert alert-danger mt-3">Ошибка создания счета</div>';
                        }
                        echo "<script>setTimeout(function() {window.location.href = '/';}, 5500); </script>";
                    }
                    ?>
                </div>
            </div>
        </div>
    </div>
    <!-- Форма редактирования профиля -->
    <div id="editProfileForm" class="action-form card p-4 my-5" style="display: none;">
        <h5 class="mb-3">Редактировать профиль</h5>
        <form method="post" action="">
            <div class="form-group">
            <label for="lastName">Фамилия</label>
            <input type="text" class="form-control" id="lastName" name="lastName" placeholder="Введите фамилию" value="<?php echo htmlspecialchars($user['lastName'] ?? ''); ?>" required>
            </div>
            <div class="form-group">
            <label for="firstName">Имя</label>
            <input type="text" class="form-control" id="firstName" name="firstName" placeholder="Введите имя" value="<?php echo htmlspecialchars($user['firstName'] ?? ''); ?>" required>
            </div>
            <div class="form-group">
            <label for="middleName">Отчество</label>
            <input type="text" class="form-control" id="middleName" name="middleName" placeholder="Введите отчество" value="<?php echo htmlspecialchars($user['middleName'] ?? ''); ?>" required>
            </div>
            <div class="form-group">
            <label for="birthDate">Дата рождения</label>
            <input type="date" class="form-control" id="birthDate" name="birthDate" value="<?php echo isset($user['birthDate']) ? htmlspecialchars(date('Y-m-d', strtotime($user['birthDate']))) : ''; ?>" required>
            </div>
            <div class="form-group">
            <label for="country">Страна</label>
            <input type="text" class="form-control" id="country" name="country" placeholder="Введите страну" value="<?php echo htmlspecialchars($user['country'] ?? ''); ?>" required>
            </div>
            <div class="form-group">
            <label for="city">Город</label>
            <input type="text" class="form-control" id="city" name="city" placeholder="Введите город" value="<?php echo htmlspecialchars($user['city'] ?? ''); ?>" required>
            </div>
            <div class="form-group">
            <label for="address">Адрес</label>
            <input type="text" class="form-control" id="address" name="address" placeholder="Введите адрес" value="<?php echo htmlspecialchars($user['address'] ?? ''); ?>" required>
            </div>
            <div class="form-group">
            <label for="postalCode">Почтовый индекс</label>
            <input type="text" class="form-control" id="postalCode" name="postalCode" placeholder="Введите почтовый индекс" value="<?php echo htmlspecialchars($user['postalCode'] ?? ''); ?>" required>
            </div>
            <button type="submit" class="btn btn-warning mt-3" name="updateProfile">Сохранить изменения</button>
            <?php
            if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['updateProfile'])) {
            $updatedData = [
                'lastName' => $_POST['lastName'] ?? '',
                'firstName' => $_POST['firstName'] ?? '',
                'middleName' => $_POST['middleName'] ?? '',
                'birthDate' => $_POST['birthDate'] ?? '',
                'country' => $_POST['country'] ?? '',
                'city' => $_POST['city'] ?? '',
                'address' => $_POST['address'] ?? '',
                'postalCode' => $_POST['postalCode'] ?? ''
            ];

            $response = updateUserProfile($updatedData, $_COOKIE['token']);

            if ($response) {
                echo '<div class="alert alert-success mt-3">Профиль успешно обновлен!</div>';
            } else {
                echo '<div class="alert alert-danger mt-3">Ошибка обновления профиля</div>';
            }
            echo "<script>window.location.href = '/';</script>";
            }
            ?>
        </form>
    </div>
</div>

<div class="container mt-4">
    <div class="row">
        <div class="col-md-12 text-center">
            <h5 class="text-highlight mb-4">Операции над вашими счетами</h5>
            <div class="d-flex flex-wrap justify-content-center">
                <button class="btn btn-warning m-2" style="flex: 1 1 auto; min-width: 150px;" onclick="showForm('depositForm')">Пополнить счет</button>
                <button class="btn btn-warning m-2" style="flex: 1 1 auto; min-width: 150px;" onclick="showForm('withdrawForm')">Снять со счета</button>
                <button class="btn btn-warning m-2" style="flex: 1 1 auto; min-width: 150px;" onclick="showForm('transferForm')">Перевести на другой счет</button>
                <button class="btn btn-warning m-2" style="flex: 1 1 auto; min-width: 150px;" onclick="showForm('viewAccountsForm')">Просмотр моих счетов</button>
            </div>
        </div>
    </div>

    <div class="row mt-4">
        <div class="col-md-12">
            <!-- Форма пополнения -->
            <div id="depositForm" class="action-form card p-4 my-5" style="display: none;">
                <h5 class="mb-3">Пополнить счет</h5>
                <form method="post" action="">
                    <div class="form-group">
                        <label for="accountNumberDeposit">Номер счета</label>
                        <input type="text" class="form-control" id="accountNumberDeposit" name="accountNumberDeposit" placeholder="Введите номер счета" required>
                    </div>
                    <div class="form-group">
                        <label for="depositAmount">Сумма</label>
                        <input type="number" class="form-control" id="depositAmount" name="depositAmount" placeholder="Введите сумму" required>
                    </div>
                    <button type="submit" class="btn btn-warning mt-3" name="replenishAccount">Пополнить</button>
                </form>
                <?php
                if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['replenishAccount'])) {
                    $accountNumber = $_POST['accountNumberDeposit'] ?? '';
                    $amount = $_POST['depositAmount'] ?? 0;

                    if (!empty($accountNumber) && $amount > 0) {
                        $response = replenishAccount($accountNumber, (int)$amount, $_COOKIE['token']);

                        if ($response) {
                            echo '<div class="alert alert-success mt-3">Счет успешно пополнен!</div>';
                        } else {
                            echo '<div class="alert alert-danger mt-3">Ошибка пополнения счета</div>';
                        }
                    } else {
                        echo '<div class="alert alert-warning mt-3">Пожалуйста, заполните все поля корректно</div>';
                    }
                    echo "<script>setTimeout(function() {window.location.href = '/';}, 1500); </script>";
                }

                
                ?>
            </div>

            <!-- Форма снятия -->
            <div id="withdrawForm" class="action-form card p-4 my-5" style="display: none;">
                <h5 class="mb-3">Снять со счета</h5>
                <form method="post" action="">
                    <div class="form-group">
                        <label for="accountNumberWithdraw">Номер счета</label>
                        <input type="text" class="form-control" id="accountNumberWithdraw" name="accountNumberWithdraw" placeholder="Введите номер счета" required>
                    </div>
                    <div class="form-group">
                        <label for="withdrawAmount">Сумма</label>
                        <input type="number" class="form-control" id="withdrawAmount" name="withdrawAmount" placeholder="Введите сумму" required>
                    </div>
                    <button type="submit" class="btn btn-warning mt-3" name="withdrawFromAccount">Снять</button>
                </form>
                <?php
                if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['withdrawFromAccount'])) {
                    $accountNumber = $_POST['accountNumberWithdraw'] ?? '';
                    $amount = $_POST['withdrawAmount'] ?? 0;

                    if (!empty($accountNumber) && $amount > 0) {
                        $response = withdrawFromAccount($accountNumber, (int)$amount, $_COOKIE['token']);

                        if ($response) {
                            echo '<div class="alert alert-success mt-3">Сумма успешно снята со счета!</div>';
                        } else {
                            echo '<div class="alert alert-danger mt-3">Ошибка снятия со счета</div>';
                        }
                    } else {
                        echo '<div class="alert alert-warning mt-3">Пожалуйста, заполните все поля корректно</div>';
                    }
                    echo "<script>setTimeout(function() {window.location.href = '/';}, 1500); </script>";
                }
                ?>
            </div>

            <!-- Форма перевода -->
            <div id="transferForm" class="action-form card p-4 my-5" style="display: none;">
                <h5 class="mb-3">Перевести на другой счет</h5>
                <form method="post" action="">
                    <div class="form-group">
                        <label for="phoneNumberTransfer">Номер телефона получателя</label>
                        <input type="text" class="form-control" id="phoneNumberTransfer" name="phoneNumberTransfer" placeholder="Введите номер телефона" required>
                    </div>
                    <div class="form-group">
                        <label for="accountNumberTransfer">Номер вашего счета</label>
                        <input type="text" class="form-control" id="accountNumberTransfer" name="accountNumberTransfer" placeholder="Введите номер счета" required>
                    </div>
                    <div class="form-group">
                        <label for="transferAmount">Сумма</label>
                        <input type="number" class="form-control" id="transferAmount" name="transferAmount" placeholder="Введите сумму" required>
                    </div>
                    <button type="submit" class="btn btn-warning mt-3" name="transferToAccount">Перевести</button>
                </form>
                <?php
                if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['transferToAccount'])) {
                    $phoneNumber = $_POST['phoneNumberTransfer'] ?? '';
                    $accountNumber = $_POST['accountNumberTransfer'] ?? '';
                    $amount = $_POST['transferAmount'] ?? 0;

                    if (!empty($phoneNumber) && !empty($accountNumber) && $amount > 0) {
                        $response = transferToAccount($phoneNumber, $accountNumber, (int)$amount, $_COOKIE['token']);

                        if ($response) {
                            echo '<div class="alert alert-success mt-3">Перевод успешно выполнен!</div>';
                        } else {
                            echo '<div class="alert alert-danger mt-3">Ошибка перевода</div>';
                        }
                    } else {
                        echo '<div class="alert alert-warning mt-3">Пожалуйста, заполните все поля корректно</div>';
                    }
                    echo "<script>setTimeout(function() {window.location.href = '/';}, 5500); </script>";
                }
                ?>
            </div>

            <!-- Форма просмотра счетов -->
            <div id="viewAccountsForm" class="action-form card p-4 my-5" style="display: none;">
                <h5 class="mb-3">Просмотр моих счетов</h5>
                
                <form method="post" action="#">
                    <button type="submit" class="btn btn-warning mb-3" name="refreshAccounts">Обновить</button>
                </form>
                <div class="table-responsive">
                    <table class="table table-dark table-hover table-sm">
                        <thead>
                            <tr>
                                <th>Название</th>
                                <th>Номер счета</th>
                                <th>Тип</th>
                                <th>Состояние</th>
                                <th>Баланс</th>
                            </tr>
                        </thead>
                        <tbody id="accountsTableBody">
                            <?php
                            if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['refreshAccounts'])) {
                                $accounts = getAccounts($_COOKIE['token']);
                            }
                            if (is_array($accounts) && count($accounts) > 0) {
                                foreach ($accounts as $account) {
                                    $typeMapping = [
                                        0 => "Дебетовый",
                                        1 => "Кредитовый",
                                        2 => "Вклад",
                                        3 => "Инвестиции"
                                    ];
                                    $stateMapping = [
                                        0 => "Открыт",
                                        1 => "Закрыт"
                                    ];

                                    echo "<tr>";
                                    echo "<td>" . htmlspecialchars($account['name']) . "</td>";
                                    echo "<td>" . htmlspecialchars($account['id']) . "</td>";
                                    echo "<td>" . htmlspecialchars($typeMapping[$account['type']] ?? 'Неизвестный тип') . "</td>";
                                    echo "<td>" . htmlspecialchars($stateMapping[$account['state']] ?? 'Неизвестное состояние') . "</td>";
                                    echo "<td>" . htmlspecialchars($account['balance']) . " ₽</td>";
                                    echo "</tr>";
                                }
                            } else {
                                echo "<tr><td colspan='5'>Нет доступных счетов</td></tr>";
                            }
                            ?>
                        </tbody>
                    </table>
                </div>
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
    function hideForm(formId) {
            document.getElementById(formId).style.display = 'none';
        }

        document.querySelector('.btn.btn-custom').addEventListener('click', function () {
            const editProfileForm = document.getElementById('editProfileForm');
            if (editProfileForm.style.display === 'none' || editProfileForm.style.display === '') {
                showForm('editProfileForm');
            } else {
                hideForm('editProfileForm');
            }
        });
</script>
