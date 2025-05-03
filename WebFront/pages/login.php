<div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header text-center">
                    <h4>Login</h4>
                </div>
                <div class="card-body">
                    <form action="" method="POST">
                        <div class="form-group mb-3">
                            <label for="email">Email</label>
                            <input type="email" class="form-control" id="email" name="email" placeholder="Enter your email" required>
                        </div>
                        <div class="form-group mb-3">
                            <label for="password">Password</label>
                            <input type="password" class="form-control" id="password" name="password" placeholder="Enter your password" required>
                        </div>
                        <button type="submit" class="btn btn-primary w-100">Login</button>
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
            echo '<div class="alert alert-success text-center mt-3">Login successful!</div>';
        } else {
            echo '<div class="alert alert-danger text-center mt-3">Invalid email or password.</div>';
        }
    } else {
        echo '<div class="alert alert-danger text-center mt-3">Login function not found.</div>';
    }
}
?>
