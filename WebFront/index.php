<?php
include 'scripts.php';
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>CDR Bank</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="assets/css/style.css">
</head>
<body>
    <header>
        <?php include 'blocks\nav.php'; ?>
    </header>
    <main class="container mt-4">
        
        <?php

        switch ($page) {
            case 'about':
            include 'pages/about.php';
            break;
            case 'services':
            include 'pages/services.php';
            break;
            case 'contact':
            include 'pages/contact.php';
            break;
            case 'login':
            include 'pages/login.php';
            break;
            case 'register':
            include 'pages/register.php';
            break;
            case 'profile':
            include 'pages/profile.php';
            break;
            default:
            include 'pages/main.php';
            break;
        }
        ?>

    </main>
    <footer class="bg-dark text-white text-center py-3">
        <?php include 'blocks\footer.php'; ?>
    </footer>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>