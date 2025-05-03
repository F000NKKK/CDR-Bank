<?php
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $email = $_POST['email'] ?? '';
    $password = $_POST['password'] ?? '';
    $confirmPassword = $_POST['confirmPassword'] ?? '';

    // Проверка совпадения паролей
    if ($password !== $confirmPassword) {
        $errorMessage = "Пароли не совпадают.";
    }
    
    $mess = registerUser($email, $password);
    
    if (isset($_SESSION['user_logged_in']) && $_SESSION['user_logged_in']) {
        $successMessage = $mess;
    } else {
        $errorMessage = $mess;
    }
}
?>
<div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header text-center">
                    <h4>Регистрация</h4>
                </div>
                <div class="card-body">
            <form id="registerForm" method="POST">
                <div class="mb-3">
                    <label for="email" class="form-label">Электронная почта:</label>
                    <input type="email" class="form-control" id="email" name="email" required>
                </div>
                <div class="mb-3">
                    <label for="password" class="form-label">Пароль:</label>
                    <input type="password" class="form-control" id="password" name="password" required>
                </div>
                <div class="mb-3">
                    <label for="confirmPassword" class="form-label">Подтвердите пароль:</label>
                    <input type="password" class="form-control" id="confirmPassword" name="confirmPassword" required>
                </div>
                <button type="submit" class="btn btn-warning w-100">Зарегистрироваться</button>
            </form>
            <?php if (!empty($errorMessage)): ?>
                <div id="errorMessage" class="text-danger mt-3"><?= htmlspecialchars($errorMessage) ?></div>
            <?php elseif (!empty($successMessage)): ?>
                <div id="successMessage" class="text-success mt-3"><?= htmlspecialchars($successMessage) ?></div>
                <script>
                    setTimeout(function() {
                        window.location.href = '/profile.php';
                    }, 2000);
                </script>
            <?php endif; ?>
            </div>
        </div>
    </div>
</div>
