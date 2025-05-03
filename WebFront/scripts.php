<?php
session_start();
// $_SESSION['user_logged_in'] = false;
// $_SESSION['page'] = 'main';
$host = "https://192.168.77.130:52339";
$page = isset($_SESSION['page']) ? $_SESSION['page'] : 'main';

function authenticateUser($email, $password) {
    global $host;
    $authUrl = $host . "/account/login";

    $data = [
        'email' => $email,
        'password' => $password
    ];

    $ch = curl_init($authUrl);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_POST, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($data));
    curl_setopt($ch, CURLOPT_HTTPHEADER, [
        'Content-Type: application/json'
    ]);
    curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, false);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);

    $response = curl_exec($ch);
    $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    if ($httpCode === 200) {
        $responseData = json_decode($response, true);
        if (isset($responseData['token'])) {
            $_SESSION['token'] = $responseData['token'];
            $_SESSION['user_logged_in'] = true;
            setcookie('token', $responseData['token'], time() + 3600, "/");

            return true;
        }
    }

    return false;
}

function getToken() {
    if (isset($_SESSION['token'])) {
        return $_SESSION['token'];
    } elseif (isset($_COOKIE['token'])) {
        return $_COOKIE['token'];
    }

    return null;
}

function registerUser($email, $password) {
    global $host;
    $registerUrl = $host . "/account/registration";

    $data = [
        'email' => $email,
        'password' => $password
    ];

    $ch = curl_init($registerUrl);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_POST, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($data));
    curl_setopt($ch, CURLOPT_HTTPHEADER, [
        'Content-Type: application/json'
    ]);
    curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, false);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);

    $response = curl_exec($ch);

    $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    if ($httpCode === 200) {
        $responseData = json_decode($response, true);
        if (isset($responseData['token'])) {
            $_SESSION['token'] = $responseData['token'];
            $_SESSION['user_logged_in'] = true;
            setcookie('token', $responseData['token'], time() + 3600, "/");

            if (isset($responseData['user'])) {
                $_SESSION['user'] = $responseData['user'];
            }

            return "Registration and authentication successful!";
        }
    }

    return "Registration failed!";
}

function getProfileBanking() {
    global $host;
    $token = getToken();
    if (!$token) {
        return "User not authenticated!";
    }

    $url = $host . "/api/profileBanking";

    $ch = curl_init($url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_HTTPHEADER, [
        'Authorization: Bearer ' . $token,
        'Content-Type: application/json'
    ]);
    curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, false);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);

    $response = curl_exec($ch);
    $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    if ($httpCode === 200) {
        return json_decode($response, true);
    }

    return "Failed to fetch profile banking!";
}

function pushMoney($accountPhone, $amount) {
    global $host;
    $token = getToken();
    if (!$token) {
        return "User not authenticated!";
    }

    $url = $host . "/api/pushMoney";

    $data = [
        'accountPhone' => $accountPhone,
        'amount' => $amount
    ];

    $ch = curl_init($url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_POST, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($data));
    curl_setopt($ch, CURLOPT_HTTPHEADER, [
        'Authorization: Bearer ' . $token,
        'Content-Type: application/json'
    ]);
    curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, false);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);

    $response = curl_exec($ch);
    $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    if ($httpCode === 200) {
        return "Money successfully deposited!";
    }

    return "Failed to deposit money!";
}

function dropMoney($accountPhone, $amount) {
    global $host;
    $token = getToken();
    if (!$token) {
        return "User not authenticated!";
    }

    $url = $host . "/api/dropMoney";

    $data = [
        'accountPhone' => $accountPhone,
        'amount' => $amount
    ];

    $ch = curl_init($url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_POST, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($data));
    curl_setopt($ch, CURLOPT_HTTPHEADER, [
        'Authorization: Bearer ' . $token,
        'Content-Type: application/json'
    ]);
    curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, false);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);

    $response = curl_exec($ch);
    $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    if ($httpCode === 200) {
        return "Money successfully withdrawn!";
    }

    return "Failed to withdraw money!";
}

function payMoney($recipientPhone, $amount) {
    global $host;
    $token = getToken();
    if (!$token) {
        return "User not authenticated!";
    }

    $url = $host . "/api/payMoney";

    $data = [
        'recipientPhone' => $recipientPhone,
        'amount' => $amount
    ];

    $ch = curl_init($url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_POST, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($data));
    curl_setopt($ch, CURLOPT_HTTPHEADER, [
        'Authorization: Bearer ' . $token,
        'Content-Type: application/json'
    ]);
    curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, false);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);

    $response = curl_exec($ch);
    $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    if ($httpCode === 200) {
        return "Money successfully transferred!";
    }

    return "Failed to transfer money!";
}

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $email = $_POST['email'] ?? '';
    $password = $_POST['password'] ?? '';

    if (authenticateUser($email, $password)) {
        // echo "Authentication successful!";
    } else {
        // echo "Authentication failed!";
    }
}
?>
