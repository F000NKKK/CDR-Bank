<?php
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $email = $_POST['email'] ?? '';
    $password = $_POST['password'] ?? '';
    $confirmPassword = $_POST['confirmPassword'] ?? '';
    $lastName = $_POST['lastName'] ?? '';
    $firstName = $_POST['firstName'] ?? '';
    $middleName = $_POST['middleName'] ?? '';
    $birthDate = $_POST['birthDate'] ?? '';
    $phone = $_POST['phone'] ?? '';
    $country = $_POST['country'] ?? '';
    $city = $_POST['city'] ?? '';
    $address = $_POST['address'] ?? '';
    $postalCode = $_POST['postalCode'] ?? '';

    // Проверка совпадения паролей
    if ($password !== $confirmPassword) {
        $errorMessage = "Пароли не совпадают.";
    }
    
    $mess = registerUser([
        'email' => $email,
        'password' => $password,
        'lastName' => $lastName,
        'firstName' => $firstName,
        'middleName' => $middleName,
        'birthDate' => $birthDate,
        'phone' => $phone,
        'country' => $country,
        'city' => $city,
        'address' => $address,
        'postalCode' => $postalCode
    ]);
    
    if (isset($_SESSION['user_logged_in']) && $_SESSION['user_logged_in']) {
        $successMessage = $mess;
    } else {
        $errorMessage = $mess;
    }
}
?>
<div class="container my-5">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header text-center">
                    <h4>Регистрация</h4>
                </div>
                <div class="card-body">
            <form id="registerForm" method="POST">
                <div class="row mb-3 border-bottom pb-3">
                    <!-- Блок 1: Фамилия, Имя, Отчество, Дата рождения -->
                    <div class="col-md-6 mb-3">
                        <label for="lastName" class="form-label">Фамилия:</label>
                        <input type="text" class="form-control" id="lastName" name="lastName" required>
                    </div>
                    <div class="col-md-6 mb-3">
                        <label for="firstName" class="form-label">Имя:</label>
                        <input type="text" class="form-control" id="firstName" name="firstName" required>
                    </div>
                    <div class="col-md-6 mb-3">
                        <label for="middleName" class="form-label">Отчество:</label>
                        <input type="text" class="form-control" id="middleName" name="middleName" required>
                    </div>
                    <div class="col-md-6 mb-3">
                        <label for="birthDate" class="form-label">Дата рождения:</label>
                        <input type="date" class="form-control" id="birthDate" name="birthDate" required>
                    </div>
                </div>

                <div class="row mb-3 border-bottom pb-3">
                    <!-- Блок 2: Адрес, Страна, Город, Почтовый индекс -->
                    <div class="col-md-6 mb-3">
                        <label for="country" class="form-label">Страна:</label>
                        <input type="text" class="form-control" id="country" name="country" required>
                    </div>
                    <div class="col-md-6 mb-3">
                        <label for="city" class="form-label">Город:</label>
                        <input type="text" class="form-control" id="city" name="city" required>
                    </div>
                    <div class="col-md-6 mb-3">
                        <label for="address" class="form-label">Адрес:</label>
                        <input type="text" class="form-control" id="address" name="address" required>
                    </div>
                    <div class="col-md-6 mb-3">
                        <label for="postalCode" class="form-label">Почтовый индекс:</label>
                        <input type="text" class="form-control" id="postalCode" name="postalCode" required>
                    </div>
                </div>

                <div class="row">
                    <!-- Блок 3: Электронная почта, Телефон, Пароли -->
                    <div class="col-md-6">
                        <div class="mb-3">
                            <label for="email" class="form-label">Электронная почта:</label>
                            <input type="email" class="form-control" id="email" name="email" required>
                        </div>
                        <div class="mb-3">
                            <label for="phone" class="form-label">Телефон:</label>
                            <input type="tel" class="form-control" id="phone" name="phone" required>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="mb-3">
                            <label for="password" class="form-label">Пароль:</label>
                            <input type="password" class="form-control" id="password" name="password" required>
                        </div>
                        <div class="mb-3">
                            <label for="confirmPassword" class="form-label">Подтвердите пароль:</label>
                            <input type="password" class="form-control" id="confirmPassword" name="confirmPassword" required>
                        </div>
                    </div>
                </div>

                <button type="submit" class="btn btn-warning w-100">Зарегистрироваться</button>
            </form>
            <?php if (!empty($errorMessage)): ?>
                <div id="errorMessage" class="alert alert-danger mt-3" role="alert">
                    <?= htmlspecialchars($errorMessage) ?>
                </div>
            <?php elseif (!empty($successMessage)): ?>
                <div id="successMessage" class="alert alert-success mt-3" role="alert">
                    <?= htmlspecialchars($successMessage) ?>
                </div>
                <?php $_SESSION['page'] = "profile"; ?>
                <script>
                    setTimeout(function() {
                        window.location.href = '/';
                    }, 1500);
                </script>
            <?php endif; ?>
            </div>
        </div>
    </div>
</div>
