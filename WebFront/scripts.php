<?php
ob_start();
session_start();
// Start the session to manage user authentication and session data

// Define the host URL for the API
$hostIdentity = "https://localhost:52339";
$hostBanking = "https://localhost:7297";
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
    global $hostIdentity;
    $authUrl = $hostIdentity . "/account/login";

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
    global $hostIdentity;
    $registerUrl = $hostIdentity . "/account/registration";

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
    global $hostIdentity;

    $url = $hostIdentity . "/account/edit";

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

function getUserProfile($token) {
    global $hostIdentity;
    $url = $hostIdentity . "/account/get-user-contact-info";

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

function createAccount($params, $token) {
    global $hostBanking;
    $url = $hostBanking . "/banking/bank-account/open";

    $data = [
        'name' => $params['name'] ?? '',
        'type' => $params['type'] ?? '',
        'creditLimit' => $params['creditLimit'] ?? '',
        'isMain' => $params['isMain'] ?? '',
    ];

    // var_dump($data); // Debugging output

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

    // var_dump($response); // Debugging output
    if ($httpCode === 200) {
        return true;
    }

    return false;
}

function getAccounts($token, $page = 1, $pageSize = 5) {
    global $hostBanking;
    $url = $hostBanking . "/banking/bank-accounts?page=$page&pageSize=$pageSize";
    $headers = [
        'Authorization: Bearer ' . $token,
        'Content-Type: application/json'
    ];

    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, $url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
    
    curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, false);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);

    $response = curl_exec($ch);
    curl_close($ch);

    return json_decode($response, true)['items'];
}

function getBalance($token) {
    global $hostBanking;
    $url = $hostBanking . "/banking/balance";

    $ch = curl_init($url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_HTTPHEADER, [
        'Authorization: Bearer ' . $token,
        'Content-Type: application/json'
    ]);
    curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, false);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);

    $response = curl_exec($ch);
    curl_close($ch);

    return $response;
}

function replenishAccount( $bankingAccount, $amount, $token) {
    // Пример реализации функции пополнения счета
    global $hostBanking;
    $url = $hostBanking . "/banking/replenish";

    $data = [
        'bankingAccount' => $bankingAccount,
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
        return true;
    }
    
    return false;
}

function getAccountTransactions($accountId, $token, $page = 1, $pageSize = 5) {
    global $hostBanking;
    $url = $hostBanking . "/banking/transactions?BankingAccountId=$accountId&Page=$page&PageSize=$pageSize";
    $headers = [
        'Authorization: Bearer ' . $token,
        'Content-Type: application/json'
    ];

    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, $url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
    
    curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, false);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);

    $response = curl_exec($ch);
    curl_close($ch);

    $decodedResponse = json_decode($response, true);
    return !empty($decodedResponse['items']) ? $decodedResponse['items'] : [];
}


function withdrawFromAccount( $bankingAccount, $amount, $token) {
    // Пример реализации функции пополнения счета
    global $hostBanking;
    $url = $hostBanking . "/banking/withdraw";

    $data = [
        'bankingAccount' => $bankingAccount,
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
        return true;
    }
    
    return false;
}

function transferToAccount($phoneNumber, $bankingAccount, $amount, $token) {
    // Пример реализации функции пополнения счета
    global $hostBanking;
    $url = $hostBanking . "/banking/transfer";

    $data = [
        'bankingAccount' => $bankingAccount,
        'amount' => $amount,
        'recipientTelephoneNumber' => $phoneNumber,
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
    // var_dump($response); // Debugging output
    $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);
    if ($httpCode === 200) {
        return true;
    }
    
    return false;
}
?>
