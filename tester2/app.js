

// we are lazy so we use global context
var type = null;
var hashalg = null;
var signatureType = null;
var keyusage = null;
var issuer = null;
var content = null;
var digest = null;

// some init stuff
jQuery(window).load(function () {
        $("#firefox").hide();
    });

function sign(data,sigalg) {

  $("#firefox").hide();

  var port = $("#port").val();

  $("#sign").attr("disabled","disabled");
 
  content = data;
  digest = null;

  var md;
  if (type == "digest") {
     if (sigalg == "SHA1") {
         md = new jsSHA("SHA-1","B64");
     } else if (sigalg == "SHA384") {
         md = new jsSHA("SHA-384","B64");
     } else if (sigalg == "SHA512") {
         md = new jsSHA("SHA-512","B64");
     } else {
         md = new jsSHA("SHA-256","B64");
     }
     md.update(data);
     data = md.getHash("B64");
     digest = data;
  }

  if (signatureType==null) 
      signatureType = "signature";

  var request = {
    selector: {},
    content: data,
    contentType: type,
    hashAlgorithm: sigalg,
    signatureType: signatureType,
    version: "1.0"
  };

  console.log(request);


  if (keyusage!=null) {
     request.selector.keyusages = new Array();
     request.selector.keyusages.push(keyusage);
  }
  if (issuer!=null) {
     request.selector.issuers = new Array();
     request.selector.issuers.push(issuer);
  }

  SCS.sign(handleSignatureResponse,request);
  

}

function rep(ar) {
    var str = ar.join();
    return "<br/>"+str.replace(/\,/g,"<br/>");
}

function handleSignatureResponse(response) {
  console.log(response);
  if (response.status=="ok") {
    var div = $("#result");
    div.html("<p><b>"+response.reasonText+"</b> <font id=\"signature_result\"><span class='label label-warning'>validating signature on server...</span></font></p>");
    div.append("<p><b>Used SCS URL:</b> "+SCS.getURL()+"</p>");
    div.append("<p><b>Available SCS URLs:</b> "+rep(SCS.getAvailableURLs())+"</p>");
    if (response.signatureType=="signature") {
      div.append("<p><pre>Signature ("+response.signatureAlgorithm+"):\n"+rowlize(response.signature)+"</pre></p>");
    } else if (response.signatureType=="pkcs7") {
      if (type=="digest") {
        div.append("<p><pre>PKCS#7 detached ("+response.signatureAlgorithm+"):\n"+rowlize(response.signature)+"</pre></p>");
      } else {
	div.append("<p><pre>PKCS#7 attached ("+response.signatureAlgorithm+"):\n"+rowlize(response.signature)+"</pre></p>");
      }
    } else if (response.signatureType=="xml") {
	div.append("<p><pre>XML Signature ("+response.signatureAlgorithm+"):\n"+atob(response.signature).replace(/</g,"&lt;").replace(/>/g,"&gt;")+"</pre></p>");
    }
    for (var i=0;i<response.chain.length;i++)
      div.append("<p><pre>Certificate#"+i+":\n"+rowlize(response.chain[i])+"</pre></p>");
    response.content = content;
    if (digest!=null) response.digest = digest;
    response.contentType = type;
    validate(response);
  } else {
    $("#result").html("<p><span class='label label-danger'>"+response.reasonCode+"</span> "+response.reasonText+"</p>");
    if (isFirefox() && !SCS.isAvailable()) {
      $("#firefox").show();
    }
  }
  
  $("#sign").removeAttr("disabled");
}

function isFirefox() {
  if(navigator.userAgent.toLowerCase().indexOf('firefox') > -1)
  {
    return true;
  } else {
    return false;
  }
}

function validate(response) {
    if (response.signatureType=='xml') {
       $("#signature_result").html("<span class='label label-danger'>xml signature validation not supported yet</span>");
    } else {
  $.ajax({
    url: SCS.utl+"/validate",
    type: "POST",
    crossDomain: true,
    data: JSON.stringify(response),
    contentType: "application/json",
    dataType: "json"
  })
    .done(function (resp) {
      console.log("validate returns...");
      console.log(resp);
      var font = $("#signature_result");
      if (resp.result) {
	font.html("<span class='label label-success'>valid signature</span>");
      } else {
        font.html("<span class='label label-danger'>invalid signature</span>");
      }
    });
    }
}

function test() {
  $("#result").html("<p><i class='fa fa-refresh fa-spin'></i> <b>Requesting signature...</b></p>");
  sign(btoa($("#data").val()),hashalg);
}

function rowlize(str) {
   var str2 = "";
   var len = 0;
   while (str.length>0) {
      len = Math.min(64,str.length);
      str2 = str2 + str.substring(0,len) +"\n";
      str = str.substring(len);
   }
   return str2;
}

function setMethod(value) {
  method = value;
}

function setType(value) {
  type = value;
}

function setSignatureType(type) {
   signatureType = type;
}

function setSignatureAlgorithmType(alg) {
   hashalg = alg;
}

function setKeyUsage(ku) {
   keyusage = ku;
}

function setIssuer(i) {
   issuer = i;
}

function change() {
  if (window.location.protocol != "https:") {
      window.location.href = "https:" + window.location.href.substring(window.location.protocol.length);
  } else {
      window.location.href = "http:" + window.location.href.substring(window.location.protocol.length);
  }
}

function checkAvailability() {

  $("#sign").html("Checking SCS availability").attr("disabled","disabled");
  
  setTimeout(function() {
    if (SCS.isAvailable()) {
      $("#sign").html("Test SCS signing").removeAttr("disabled");
    } else {
      $("#sign").html("SCS not available").attr("disabled","disabled");
    }
  }, 1000);

}

$(document).ready(function()  {
   //$("#buttonSHA1withRSA").button("toggle");
   //checkAvailability();
   type = "data";
   hashalg = null;
   signatureType = null;
   keyusage = null;
   issuer = null;
   $("#data").append(new Date().toString());

   if (window.location.protocol != "https:") {
       $("#buttontitle").html("Load this page over HTTPS");
       $(".pagetitle").html("SCS Tester (over HTTP)");
   } else {
       $("#buttontitle").html("Load this page over HTTP");
       $(".pagetitle").html("SCS Tester (over HTTPS)");
   }
});

