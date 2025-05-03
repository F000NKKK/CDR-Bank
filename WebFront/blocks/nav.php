<?php
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['page'])) {
    $_SESSION['page'] = $_POST['page'];
    echo '<script>window.location.href = "' . $_SERVER['PHP_SELF'] . '";</script>';
    exit;
}
?>
<!-- Навигационное меню с темной темой -->
<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
    <div class="container-fluid">
        <form method="post" style="display:inline;">
            <button class="navbar-brand nav-link" type="submit" name="page" value="main">CDR BANK</button>
        </form>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Переключить навигацию">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav me-auto">
                <li class="nav-item">
                    <form method="post" style="display:inline;">
                        <button class="nav-link <?php if (isset($_SESSION['page']) && $_SESSION['page'] === 'main') { echo 'active'; } ?> btn btn-link" type="submit" name="page" value="main">Главная</button>
                    </form>
                </li>
                <li class="nav-item">
                    <form method="post" style="display:inline;">
                        <button class="nav-link <?php if (isset($_SESSION['page']) && $_SESSION['page'] === 'about') { echo 'active'; } ?> btn btn-link" type="submit" name="page" value="about">О нас</button>
                    </form>
                </li>
                <li class="nav-item">
                    <form method="post" style="display:inline;">
                        <button class="nav-link <?php if (isset($_SESSION['page']) && $_SESSION['page'] === 'services') { echo 'active'; } ?> btn btn-link" type="submit" name="page" value="services">Услуги</button>
                    </form>
                </li>
                <li class="nav-item">
                    <form method="post" style="display:inline;">
                        <button class="nav-link <?php if (isset($_SESSION['page']) && $_SESSION['page'] === 'contact') { echo 'active'; } ?> btn btn-link" type="submit" name="page" value="contact">Контакты</button>
                    </form>
                </li>
            </ul>
            <ul class="navbar-nav">
                <?php if (isset($_SESSION['user_logged_in']) && $_SESSION['user_logged_in'] === true): ?>
                    <li class="nav-item">
                        <form method="post" style="display:inline;">
                            <button class="nav-link <?php if (isset($_SESSION['page']) && $_SESSION['page'] === 'profile') { echo 'active'; } ?> btn btn-link" type="submit" name="page" value="profile">Профиль</button>
                        </form>
                    </li>
                    <li class="nav-item">
                        <form method="post" style="display:inline;">
                            <button class="nav-link btn btn-link" type="submit" name="page" value="logout">Выйти</button>
                            <?php
                            if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['page']) && $_POST['page'] === 'logout') {
                                // Очистка данных сессии
                                
                                $_SESSION['user_logged_in'] === false;
                                session_unset();
                                session_destroy();

                                // Очистка всех cookies
                                if (isset($_SERVER['HTTP_COOKIE'])) {
                                    $cookies = explode('; ', $_SERVER['HTTP_COOKIE']);
                                    foreach ($cookies as $cookie) {
                                        $parts = explode('=', $cookie);
                                        $name = trim($parts[0]);
                                        setcookie($name, '', time() - 3600, '/');
                                    }
                                }

                                // Перенаправление на страницу входа или главную
                                echo '<script>window.location.href = "' . $_SERVER['PHP_SELF'] . '";</script>';
                                exit;
                            }
                            ?>
                        </form>
                    </li>
                <?php else: ?>
                    <li class="nav-item">
                        <form method="post" style="display:inline;">
                            <button class="nav-link <?php if (isset($_SESSION['page']) && $_SESSION['page'] === 'login') { echo 'active'; } ?> btn btn-link" type="submit" name="page" value="login">Войти</button>
                        </form>
                    </li>
                    <li class="nav-item">
                        <form method="post" style="display:inline;">
                            <button class="nav-link <?php if (isset($_SESSION['page']) && $_SESSION['page'] === 'register') { echo 'active'; } ?> btn btn-link" type="submit" name="page" value="register">Регистрация</button>
                        </form>
                    </li>
                <?php endif; ?>
        </div>
    </div>
</nav>