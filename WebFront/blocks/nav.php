<!-- Навигационное меню с темной темой -->
<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
    <div class="container-fluid">
        <a class="navbar-brand" href="index.php">CDR Bank</a>
        <form method="post" style="display:inline;">
            <button class="navbar-toggler" type="submit" name="page" value="toggle_nav" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
            </button>
            <?php
            if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['page']) && $_POST['page'] === 'toggle_nav') {
            $_SESSION['nav_toggled'] = !isset($_SESSION['nav_toggled']) || !$_SESSION['nav_toggled'];
            }
            ?>
        </form>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav me-auto">
                <li class="nav-item">
                    <form method="post" style="display:inline;">
                        <button class="nav-link <?php if (isset($_SESSION['page']) && $_SESSION['page'] === 'main') { echo 'active'; } ?> btn btn-link" type="submit" name="page" value="main">Home</button>
                    </form>
                </li>
                <li class="nav-item">
                    <form method="post" style="display:inline;">
                        <button class="nav-link <?php if (isset($_SESSION['page']) && $_SESSION['page'] === 'about') { echo 'active'; } ?> btn btn-link" type="submit" name="page" value="about">About Us</button>
                    </form>
                </li>
                <li class="nav-item">
                    <form method="post" style="display:inline;">
                        <button class="nav-link <?php if (isset($_SESSION['page']) && $_SESSION['page'] === 'services') { echo 'active'; } ?> btn btn-link" type="submit" name="page" value="services">Services</button>
                    </form>
                </li>
                <li class="nav-item">
                    <form method="post" style="display:inline;">
                        <button class="nav-link <?php if (isset($_SESSION['page']) && $_SESSION['page'] === 'contact') { echo 'active'; } ?> btn btn-link" type="submit" name="page" value="contact">Contact</button>
                    </form>
                </li>
            </ul>
            <ul class="navbar-nav">
                <?php if (isset($_SESSION['user_logged_in']) && $_SESSION['user_logged_in'] === true): ?>
                    <li class="nav-item">
                        <form method="post" style="display:inline;">
                            <button class="nav-link <?php if (isset($_SESSION['page']) && $_SESSION['page'] === 'profile') { echo 'active'; } ?> btn btn-link" type="submit" name="page" value="profile">Profile</button>
                        </form>
                    </li>
                    <li class="nav-item">
                        <form method="post" style="display:inline;">
                            <button class="nav-link btn btn-link" type="submit" name="page" value="logout">Logout</button>
                            <?php
                            if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['page']) && $_POST['page'] === 'logout') {
                                // Clear session data
                                session_unset();
                                session_destroy();

                                // Clear cookies
                                if (isset($_COOKIE['token'])) {
                                    setcookie('token', '', time() - 3600, '/');
                                }

                                // Redirect to the login page or home
                                header('Location: index.php');
                                exit;
                            }
                            ?>
                        </form>
                    </li>
                <?php else: ?>
                    <li class="nav-item">
                        <form method="post" style="display:inline;">
                            <button class="nav-link <?php if (isset($_SESSION['page']) && $_SESSION['page'] === 'login') { echo 'active'; } ?> btn btn-link" type="submit" name="page" value="login">Login</button>
                        </form>
                    </li>
                    <li class="nav-item">
                        <form method="post" style="display:inline;">
                            <button class="nav-link <?php if (isset($_SESSION['page']) && $_SESSION['page'] === 'register') { echo 'active'; } ?> btn btn-link" type="submit" name="page" value="register">Register</button>
                        </form>
                    </li>
                <?php endif; ?>
            </ul>
                        </form>
                    </li>
            </ul>

            <?php
            if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['page'])) {
                $_SESSION['page'] = $_POST['page'];
                header('Location: ' . $_SERVER['PHP_SELF']);
                exit;
            }
            ?>
        </div>
    </div>
</nav>