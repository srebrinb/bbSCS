/********************************************************************
 *   Signature Creation Service module
 *   Version 1.0.1 (2015-11-11)
 *   Copyright 2015 Vaestorekisterikeskus
 *   Pekka Laitinen <pekka.laitinen@vrk.fi>
 *
 *   SCS module requires:
 *    - jQuery: https://jquery.com/
 *
 ********************************************************************/
 
var SCS = (function($) {

        /************************************************************/
	/**** SCS module private methods ****************************/
        /************************************************************/

	// setup initial parameters
	var my = {};
	var secure_urls = [
			   "https://localhost:53952",
			   "https://localhost:23124"
			  ];
	var plain_urls  = [
		  	 "http://localhost:53951",
			   "http://localhost:23123"
			  ];
	var urls = null;
	var url = null;
        var available_urls = new Array();
	var initialized = false;
        var last_autodiscovery_check = 0;
        var version_info = new Object();
	// detect which port is in use
	console.log("SCS: initiating automatic service discovery...");
	autodiscovery(0);

        /**
         * Get last time check autodiscovery was executed
         */
        function getHowMuchSinceLastAutodiscovery() {
	  var now = new Date().getTime();
	  return now-last_autodiscovery_check;
        }

	/**
 	 * Autodiscovery of the SCS URL on localhost from defined port list.
	 */
	function autodiscovery(i) {
		/* We need to uncomment this if we cannot implement SCS normally on iOS
		if (isIOS()) {
			//port = "ios";
			console.log("SCS: SCS service is running on iOS");
			initialized = true;
			return;
		}
		*/

	        /* 
		   We need to check whether the document was retrieved
		   over http or https as CORS can typically be only be
		   done to over https if the document was retrieved over
		   https.
		 */
	        if (i==0) {
		  last_autodiscovery_check = new Date().getTime();
                  if (window.location.protocol=="https:") {
		    urls = secure_urls;
                  } else {
		    urls = plain_urls;
		  }
	        }
		if (i>=urls.length) {
		    console.log("SCS: All discovery urls probed, waiting for service discovery...");
		    return;
		}
		$.ajax({
			method: "GET",
			url: "alert://asdfasdf",
			crossDomain: true,
			dataType: "json",
			settings: { timeout: 100 }
		}).fail(function(resp) {
		 console.log("error");
		});
		console.log("asdf");
		$.ajax({
			method: "GET",
			url: urls[i]+"/version",
			crossDomain: true,
			dataType: "json",
			settings: { timeout: 100 }
		})
		.done(function(response) {
			url = urls[i]+"/sign";
			available_urls.push(url);
			initialized = true;
			init(response);
			console.log("SCS: found SCS service at "+url);
		})
		.fail(function(resp) {
			if (resp instanceof Object) {
				if (resp.status==200) {
					// We are because SCS implementation does not support 
					// version check (like early versions of Fujitsu DigiSign.
					// If status is 200 then there was a server responding,
					// but we are not 100% sure that it is SCS service.
					url = urls[i]+"/sign";
					available_urls.push(url);
					console.log("SCS: "+url);
					console.log("SCS: Found a service in "+url+", but we are not 100% sure it is SCS service. We use this port if no valid SCS service is found.");
				}
					
			}	
		});
       		autodiscovery(i+1);

	}

	/**
	 * Initialize the SCS service parameters.
	 */
	function init(response) {
		// store possible version parameters of SCS service, i.e., is
		// selector supported, SCS version, supported HTTP methods, etc.	
                version_info = response;
        }
	
	/**
	 * Check if we are on iOS device.
	 */
	function isIOS() {
		try {
			return ( navigator.userAgent.match(/(iPad|iPhone|iPod)/g) ? true : false );
		} catch (ex) {
			return false;
		}
	}
	
	/**
	 * Do SCS signing request based on request parameters.
	 */
	function sign_localhost(callback,request) {
	    console.log("URL: "+url);
		$.ajax({
			url: url,
			type: "POST",
			crossDomain: true,
			data: JSON.stringify(request),
			contentType: "application/json",
			dataType: "json"
			})
			.done(function(response) {
				doCallback(callback,response);
			})
			.fail(function() {
				var response = new Object();
				response.ok=false,
				response.reasonCode=500,
				response.reasonText="SCS not responding"
				doCallback(callback,response);
			});
	}

	/**
	 * Do SCS signing in iOS (it maybe that SCS as such is not possible in iOS)
	 */
	function sign_ios(callback,request) {

		//  We need to implement this part if SCS cannot be implemented on iOS

		/*  This is for the "not implemented notification
		var response = new Object();
		response.ok=false,
		response.reasonCode=500,
		response.reasonText="iOS not supported"
		setTimeout(function() { doCallback(callback,response); }, 10);
		*/
		
		// But for now, we use normal SCS localhost signing
		sign_localhost(callback,request);
		
	}

	/**
	 * Check if SCS service is available.
         */
	function isAvailable() {
		return (url!=null);
	}

	/**
	 * Check if SCS module is initialized (i.e., is finished with autodiscovery of the port).
	 */
	function isInitialized() {
		return initialized;
	}
	
	/**
	 * Single point to callback application code.
	 */
	function doCallback(callback,response) {
		try {
			response = validate(response);
			callback(response);
		} catch (ex) {
			// This exception should've been catched on caller side!
			console.log(ex);
		}
	}

	function validate(response) {
		// here we can do a sanity check for the response
		return response;
	}

	function doSign(callback,content,contentType,hashAlgorithm,signatureType,selector) {
		if (!isAvailable()) {
		    if (getHowMuchSinceLastAutodiscovery()>2000) {
			autodiscovery(0);
			setTimeout(function() {
			       doSign(callback,content,contentType,hashAlgorithm,signatureType,selector);
			    },1000);
                    } else {
			console.log("SCS: SCS is not available");
			var response = new Object();
			response.ok=false,
			response.reasonCode=500;
			response.reasonText="SCS not available";
			setTimeout(function() { doCallback(callback,response); },10);
		    }
		    return;
		}

		var request = null;
		if (content instanceof Object) {
			request = content;
		} else {
			request = new Object();
			request.version="1.0";
			request.content = content;
			if (contentType) request.contentType = contentType;
			if (hashAlgorithm) request.hashAlgorithm = hashAlgorithm;
			if (signatureType) request.signatureType = signatureType;
			if (selector) request.selector = selector;
		}

		if (isIOS()) {
			console.log("SCS: Doing signature with ios...");
			sign_ios(callback,request);
		} else {
			console.log("SCS: Doing signature with SCS service...");
			sign_localhost(callback,request);
		}

        }

        /************************************************************/
	/**** SCS module methods ************************************/
        /************************************************************/

	/**
         * Check is SCS module is ready.
	 */
	my.isAvailable = function() {
		return isAvailable();
	}

	/**
	 * Request signature from SCS.
	 */
	my.sign = function(callback,content, contentType, hashAlgorithm, signatureType, selector) {
	    doSign(callback,content,contentType,hashAlgorithm,signatureType,selector);
	}

	/**
         * Set SCS port manually (e.g., in the case if port autodiscovery fails).
	 */
        my.setURL = function(newurl) {
		url = newurl;
	}

	my.getURL = function() {
	    return url;
	}

	my.getAvailableURLs = function() {
	    return available_urls;
        }

        my.getVersionInfo = function() {
            return version_info;
        }

	return my;

}(jQuery));
