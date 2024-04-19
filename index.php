<?php
$new_version = "2.0.21";
if( isset($_GET["update"]) ){
	//header("Access-Control-Allow-Origin: *");
	header('Content-type: application/xml');
	//header("Content-Type: text/xml; charset=UTF-8");	
	echo '<?xml version="1.0" encoding="UTF-8"?>';
	echo '<item>';
	echo '<version>'.$new_version.'</version>';
	echo '<url>https://vnem.com/app/download/OpenClinicDataCollection.zip</url>';
	echo '<changelog>https://vnem.com/app/download</changelog>';
	echo '<mandatory>false</mandatory>';
	echo '</item>';
	die();
}
else{
	echo "<h3>Data collection app</h3>";
	echo "<h4>New version: $new_version</h4>";
}
?>