<?php
header("Content-Type:application/json");
$payloadRequest=file_get_contents("php://input");
$payload=json_decode($payloadRequest); 

$cert=$payload->chain[0];
	$cert=base64_decode($cert);
	$data=base64_decode($payload->content);
	$signature=base64_decode($payload->signature);
	$cert="-----BEGIN CERTIFICATE-----
". chunk_split(base64_encode($cert))."-----END CERTIFICATE-----";
$cert_x509 = openssl_x509_parse($cert);


  $key=openssl_get_publickey($cert);
  $out=new stdClass();
  $hashAlg=OPENSSL_ALGO_SHA1;
  switch($payload->signatureAlgorithm){
  	case "SHA1withRSA":
	  	$hashAlg=OPENSSL_ALGO_SHA1;
	  	break;
	  case "SHA256withRSA":
	  	$hashAlg=OPENSSL_ALGO_SHA256;
	  	break;	
	  case "SHA384withRSA":
	  	$hashAlg=OPENSSL_ALGO_SHA384;
	  	break;
 	  case "SHA512withRSA":
  	$hashAlg=OPENSSL_ALGO_SHA512;
  	break;		   		  	
  	default:
  	  $hashAlg=OPENSSL_ALGO_SHA1;
  }
  $cer=print_r($cert_x509,true);
  $out->cert=$cer;
  if(openssl_verify ($data, $signature, $key,$hashAlg)){
  	$out->result=true;
   }
  else {
  	$out->result=false;
 }
 echo json_encode($out);
?>