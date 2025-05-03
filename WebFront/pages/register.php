<?php
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $email = $_POST['email'] ?? '';
    $password = $_POST['password'] ?? '';
    $confirmPassword = $_POST['confirmPassword'] ?? '';

    // Проверка совпадения паролей
    if ($password !== $confirmPassword) {
        $errorMessage = "Passwords do not match.";
    }
    
    registerUser($email, $password);
    if (isset($_SESSION['user_logged_in']) && $_SESSION['user_logged_in']) {
        $successMessage = "Registration successful! You are now logged in.";
    } else {
        $errorMessage = "Registration failed! Please try again.";
    }
}
?>
<div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header text-center">
                    <h4>Registration</h4>
                </div>
                <div class="card-body">
            <form id="registerForm" method="POST">
                <div class="mb-3">
                    <label for="email" class="form-label">Email:</label>
                    <input type="email" class="form-control" id="email" name="email" required>
                </div>
                <div class="mb-3">
                    <label for="password" class="form-label">Password:</label>
                    <input type="password" class="form-control" id="password" name="password" required>
                </div>
                <div class="mb-3">
                    <label for="confirmPassword" class="form-label">Confirm Password:</label>
                    <input type="password" class="form-control" id="confirmPassword" name="confirmPassword" required>
                </div>
                <button type="submit" class="btn btn-warning w-100">Register</button>
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
