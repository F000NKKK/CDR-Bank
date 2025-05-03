<?php
ob_start();
session_start();
// Start the session to manage user authentication and session data

// Define the host URL for the API
$host = "https://localhost:52339";
// Determine the current page from the session or default to 'main'
$page = isset($_SESSION['page']) ? $_SESSION['page'] : 'main';

/**
 * Authenticate the user with the provided email and password.
 * If successful, store the token in the session and set a cookie.
 *
 * @param string $email
 * @param string $password
 * @return bool
 */
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
            // Store the token in the session and set a cookie
            $_SESSION['token'] = $responseData['token'];
            $_SESSION['user_logged_in'] = true;

            
            setcookie('token', $responseData['token'], time() + 3600, "/");

            return true;
        }
    }

    return false;
}

/**
 * Retrieve the authentication token from the session or cookie.
 *
 * @return string|null
 */
function getToken() {
    if (isset($_SESSION['token'])) {
        setcookie('token', $_SESSION['token'], time() + 3600, "/");
        return $_SESSION['token'];
    } elseif (isset($_COOKIE['token'])) {
        return $_COOKIE['token'];
    }

    return null;
}

/**
 * Register a new user with the provided email and password.
 * If successful, store the token and user data in the session.
 *
 * @param string $email
 * @param string $password
 * @return string
 */
function registerUser($params) {
    global $host;
    $registerUrl = $host . "/account/registration";

    $data = [
        'email' => $params['email'] ?? '',
        'password' => $params['password'] ?? '',
        'lastName' => $params['lastName'] ?? '',
        'firstName' => $params['firstName'] ?? '',
        'middleName' => $params['middleName'] ?? '',
        'birthDate' => $params['birthDate'] ?? '',
        'phoneNumber' => $params['phone'] ?? '',
        'country' => $params['country'] ?? '',
        'city' => $params['city'] ?? '',
        'address' => $params['address'] ?? '',
        'postalCode' => $params['postalCode'] ?? ''
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

    $responseData = json_decode($response, true);

    // var_dump($responseData); // Debugging output

    if ($httpCode === 200) {
        $responseData = json_decode($response, true);
        if (isset($responseData['token'])) {
            // Store the token and user data in the session
            $_SESSION['token'] = $responseData['token'];
            $_SESSION['user_logged_in'] = true;
            setcookie('token', $responseData['token'], time() + 3600, "/");

            if (isset($responseData['user'])) {
                $_SESSION['user'] = $responseData['user'];
            }

            return "Registration and authentication successful!";
        }
    }
    $_SESSION['user_logged_in'] = false;
    return "Registration failed!";
}

function updateUserProfile($params, $token) {
    global $host;

    $url = $host . "/account/edit";

    $data = [
        'lastName' => $params['lastName'] ?? '',
        'firstName' => $params['firstName'] ?? '',
        'middleName' => $params['middleName'] ?? '',
        'birthDate' => $params['birthDate'] ?? '',
        'country' => $params['country'] ?? '',
        'city' => $params['city'] ?? '',
        'address' => $params['address'] ?? '',
        'postalCode' => $params['postalCode'] ?? ''
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

    var_dump($response); // Debugging output

    if ($httpCode === 200) {
        return true;
    }

    return false;
}

/**
 * Fetch the user's banking profile using the API.
 *
 * @return array|string
 */
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

/**
 * Deposit money into the specified account.
 *
 * @param string $accountPhone
 * @param float $amount
 * @return string
 */
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

/**
 * Withdraw money from the specified account.
 *
 * @param string $accountPhone
 * @param float $amount
 * @return string
 */
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

/**
 * Transfer money to another user's account.
 *
 * @param string $recipientPhone
 * @param float $amount
 * @return string
 */
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

function getUserProfile($token) {
    global $host;
    $url = $host . "/account/get-user-contact-info";

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
    
    return false;
}

?>
