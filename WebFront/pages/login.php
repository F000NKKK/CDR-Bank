<div class="container my-5">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header text-center">
                    <h4>Вход в аккаунт</h4>
                </div>
                <div class="card-body">
                    <form action="" method="POST">
                        <div class="form-group mb-3">
                            <label for="email">Электронная почта:</label>
                            <input type="email" class="form-control" id="email" name="email" placeholder="Введите свой email" required>
                        </div>
                        <div class="form-group mb-3">
                            <label for="password">Пароль:</label>
                            <input type="password" class="form-control" id="password" name="password" placeholder="Введите свой пароль" required>
                        </div>
                        <button type="submit" class="btn btn-warning w-100">Вход</button>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>

<?php
// Process the login if the form is submitted
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $email = $_POST['email'];
    $password = $_POST['password'];

    // Call the login function from script.php
    if (function_exists('authenticateUser')) {
        $loginResult = authenticateUser($email, $password);
        if ($loginResult) {
            echo '<div class="alert alert-success text-center mt-3">Вы успешно вошли!</div>';
            $_SESSION['page'] = "profile";
            echo "<script>setTimeout(function() {window.location.href = '/';}, 1500); </script>";

        } else {
            echo '<div class="alert alert-danger text-center mt-3">Некорректные почта или пароль.</div>';
        }
    } else {
        echo '<div class="alert alert-danger text-center mt-3">ERR:Login function not found.</div>';
    }
}
?>
