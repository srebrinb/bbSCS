
var method = "POST";
var validation = true;
var type = null;
var hashalg = null;
var keyusage = null;
var issuer = null;
var thumbprint = null;


function sign(data,sigalg) {

  var port = $("#port").val();

  $("#sign").attr("disabled","disabled");

  var request = {
    selector: {},
    content: data,
    contentType: type,
    hashAlgorithm: sigalg,
    signatureType: "signature",
    version: "1.0"
  };

  console.log(request);

  request.selector.validate = validation;


  if (keyusage!=null) {
     request.selector.keyusages = new Array();
     request.selector.keyusages.push(keyusage);
  }
  if (issuer!=null) {
     request.selector.issuers = new Array();
     request.selector.issuers.push(issuer);
  }
  if (thumbprint!=null){
    request.selector.thumbprint=thumbprint;
  }

  var payload = null;
  var querystr = "";
  if (method=="POST") {
     payload = JSON.stringify(request);
  } else {
     querystr =  "?version="+request.version;
     querystr += "&contentType="+escape(request.contentType);
     querystr += "&hashAlgorithm="+escape(request.hashAlgorithm);
     querystr += "&signatureType="+escape(request.signatureType);
     querystr += "&content="+escape(request.content);
  }
  
      var url = "/sign" + querystr;
  
  $.ajax({
    url: url,
    type: method,
    crossDomain: true,
    data: payload,
    contentType: "application/json",
    dataType: "json"
  })
    .done(function (response) {
      console.log(response);
      if (response.status=="ok") {
        var div = $("#result");
        div.html("<p><b>"+response.reasonText+"</b> <font id=\"signature_result\"><span class='label label-warning'>validating signature on server...</span></font></p>");
        div.append("<p><pre>Signature ("+response.signatureAlgorithm+"):\n"+rowlize(response.signature)+"</pre></p>");
        for (var i=0;i<response.chain.length;i++)
           div.append("<p><pre>Certificate#"+i+":\n"+rowlize(response.chain[i])+"</pre></p>");
        response.content = data;
        response.contentType = type;
        validate(response);
      } else
        $("#result").html("<p><span class='label label-danger'>"+response.reasonCode+"</span> "+response.reasonText+"</p>");
      $("#sign").removeAttr("disabled");
    })
    .fail(function () {
      console.log("failed");
      $("#result").html("<p><b>Failed!</b> Most likely the SCS server is not running or browser's configuration policy does not allow cross origin requests.  For instance, IE 10 and 11 do not allow requests to localhost unless the origin server is listed as trusted: see IE Settings > Internet Options > Security tab > click Trusted sites > click Sites buttons.</p>");
      $("#sign").removeAttr("disabled");
    });

}

function validate(response) {
  $.ajax({
    url: "/validate",
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

function test() {
  $("#result").html("<p><i class='fa fa-refresh fa-spin'></i> <b>Requesting signature...</b></p>");
  sign(btoa($("#data").val()),hashalg);
}

function rowlize(str) {
   var str2 = "";
   var len = 0;
   if (str==null) return str;
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

function setValidation(value) {
  validation = value;
}

function setType(value) {
  type = value;
}

function setSignatureType(alg) {
   hashalg = alg;
}

function setKeyUsage(ku) {
   keyusage = ku;
}

function setIssuer(i) {
   issuer = i;
}
function setThumbprint(i){
	thumbprint=i;
}

$(document).ready(function()  {
   //$("#buttonSHA1withRSA").button("toggle");
   type = "data";
   hashalg = null;
   keyusage = null;
   issuer = null;
   $("#data").append(new Date().toString());
});