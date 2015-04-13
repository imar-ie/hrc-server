<?php

require_once 'vendor\autoload.php';

use \Slim\Middleware\HttpBasicAuthentication\PdoAuthenticator;

$app = new \Slim\Slim();

$app->view(new \JsonApiView());
$app->add(new \JsonApiMiddleware());

$device_table = "devices";

$checkin_table = "Checkins";

// Database connection

try{
	
	$pdo = new \PDO("sqlsrv:server = tcp:cqaa8fugxp.database.windows.net,1433; Database = hrlockerapi_db", "hrlocker", "9cG1bE3EwC96rPv1");
	
	$pdo->setAttribute( PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION );//Error Handling
	
}catch(PDOException $e ){
		print( "Error connecting to SQL Server." );
		die(print_r($e));
}

// Create device Table 
//createDeviceTable();


// Add new device
//addDevice();


// auth

$app->add(new \Slim\Middleware\HttpBasicAuthentication([
    "path" => "/api",
    "realm" => "Protected",
       "authenticator" => new PdoAuthenticator([
        "pdo" => $pdo,
        "table" => $device_table,
        "user" => "ref",
        "hash" => "hash"
    ])

]));


// api endpoint


 $app->post('/api/checkin', function() use ($app) {
 	
	$device_ref = $_SERVER['PHP_AUTH_USER'];
	
	if(ISSET($_POST['payload'])){
		
	$payload = $_POST['payload'];
		
		$type = $_POST['type'];
	
	addCheckin($device_ref, $payload, $type );
 
	
    $app->render(200,array(
                'msg' => 'Success',
    ));
		
	}else{
		
	$app->render(200,array(
                'msg' => 'no playload recieved',
                'error' => true
    ));
		
	}
	

    });




$app->run();



function createDeviceTable(){
	
	global $device_table;
	
	global $pdo;
	
	
try {
     //ID INT( 11 ) AUTO_INCREMENT PRIMARY KEY,
     
     
     
     $sql ="CREATE table $device_table(
     ID INT NOT NULL IDENTITY(1, 1),
     ref VARCHAR( 50 ) NOT NULL, 
     hash VARCHAR( 255 ) NOT NULL,
     CONSTRAINT PK_Source PRIMARY KEY CLUSTERED   (        ID ASC  ));" ;
	 
	 
     $pdo->exec($sql);
     print("Created $device_table Table.\n");

} catch(PDOException $e) {
    echo $e->getMessage();//Remove or change message in production code
}


}


function createClockinTable(){
	
	global $checkin_table;
	
	global $pdo;
	
	
try {
     //ID INT( 11 ) AUTO_INCREMENT PRIMARY KEY,
     
     
     
     $sql ="CREATE table $checkin_table(
     ID INT NOT NULL IDENTITY(1, 1),
     ref VARCHAR( 50 ) NOT NULL, 
     hash VARCHAR( 255 ) NOT NULL,
     CONSTRAINT PK_Source PRIMARY KEY CLUSTERED   (        ID ASC  ));" ;
	 
     $pdo->exec($sql);
     print("Created $device_table Table.\n");

} catch(PDOException $e) {
    echo $e->getMessage();//Remove or change message in production code
}

}


function addDevice(){
	
	global $device_table;
	
	global $pdo;
	

	$user = "root";
	$hash = password_hash("toor", PASSWORD_DEFAULT);
	
	
	$sql = "INSERT INTO $device_table (\"ref\", \"hash\") VALUES ('{$user}', '{$hash}')";
	
	echo $sql;
	
	try{
		

	$status = $pdo->exec($sql);
	
	print("Inseted into $device_table Table.\n");

} catch(PDOException $e) {
    echo $e->getMessage();//Remove or change message in production code
}

}



function addCheckin($device_ref, $payload, $type){
	
	global $checkin_table;
	
	global $pdo;

	
	$sql = "INSERT INTO $checkin_table (\"Device_ref\", \"Payload\", \"Type_id\", \"Time\") VALUES ('$device_ref', '$payload', '$type', getdate())";
	

	try{
	
	$status = $pdo->exec($sql);
	

} catch(PDOException $e) {
    echo $e->getMessage();//Remove or change message in production code
}

}




