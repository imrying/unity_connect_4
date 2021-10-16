<?php
$con = mysqli_connect ('localhost', 'root', '', 'unityaccess');

if (mysqli_connect_errno())
{
    echo "1: Connection failed"; //error code #1
    exit();
}

$username = $_POST["name"];
$password = $_POST["password"];

//check if name exists
$namecheckquery = "SELECT username, salt, hash, score FROM players WHERE username='" . $username . "';";

$namecheck = mysqli_query($con, $namecheckquery) or die("2: Name check query failed"); //error code #2


if (mysqli_num_rows($namecheck) != 1)
{
    echo "5: Either no user with name or more than one"; //error code #3
    exit();
}

$existinginfo = mysqli_fetch_assoc($namecheck);
$salt = $existinginfo["salt"];
$hash = $existinginfo["hash"];

$loginhash = crypt($password, $salt);

if ($hash != $loginhash)
{
    echo "6: Incorrect password";
    exit();
}

echo "0\t" . $existinginfo["score"];
?>