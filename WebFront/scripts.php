<?php
session_start();
//$_SESSION['user_logged_in'] = true; 
$page = isset($_SESSION['page']) ? $_SESSION['page'] : 'main';


function authenticateUser($email, $password) {
    // Replace with your API endpoint
    $authUrl = "https://cdr-bank.voluntas-progresus.tech/api/auth";

    // Prepare the request payload
    $data = [
        'email' => $email,
        'password' => $password
    ];

    // Initialize cURL
    $ch = curl_init($authUrl);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_POST, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($data));
    curl_setopt($ch, CURLOPT_HTTPHEADER, [
        'Content-Type: application/json'
    ]);

    // Execute the request
    $response = curl_exec($ch);
    $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    // Handle the response
    if ($httpCode === 200) {
        $responseData = json_decode($response, true);
        if (isset($responseData['token'])) {
            // Save token in session and cookie
            $_SESSION['token'] = $responseData['token'];
            $_SESSION['user_logged_in'] = true; // Set user_logged_in flag
            setcookie('token', $responseData['token'], time() + 3600, "/"); // 1-hour expiration

            return true;
        }
    }

    return false;
}

function getToken() {
    // Check if token exists in session or cookie
    if (isset($_SESSION['token'])) {
        return $_SESSION['token'];
    } elseif (isset($_COOKIE['token'])) {
        return $_COOKIE['token'];
    }

    return null;
}

function registerUser($email, $password) {
    // Replace with your API endpoint
    $registerUrl = "https://cdr-bank.voluntas-progresus.tech/api/register";

    // Prepare the request payload
    $data = [
        'email' => $email,
        'password' => $password
    ];

    // Initialize cURL
    $ch = curl_init($registerUrl);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_POST, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($data));
    curl_setopt($ch, CURLOPT_HTTPHEADER, [
        'Content-Type: application/json'
    ]);

    // Execute the request
    $response = curl_exec($ch);
    $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    // Handle the response
    if ($httpCode === 201) { // Assuming 201 Created for successful registration
        $responseData = json_decode($response, true);
        if (isset($responseData['token'])) {
            // Save token in session and cookie
            $_SESSION['token'] = $responseData['token'];
            $_SESSION['user_logged_in'] = true; // Set user_logged_in flag
            setcookie('token', $responseData['token'], time() + 3600, "/"); // 1-hour expiration

            // Save other user data if needed
            if (isset($responseData['user'])) {
                $_SESSION['user'] = $responseData['user'];
            }

            return "Registration and authentication successful!";
        }
    }

    return "Registration failed!";
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