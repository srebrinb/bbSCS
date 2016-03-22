#include <csignal>
#include <iostream>
#include "server/server_https.hpp"

//Added for the json-example
#define BOOST_SPIRIT_THREADSAFE
#include <boost/property_tree/ptree.hpp>
#include <boost/property_tree/json_parser.hpp>


#include <signal.h>

//Added for the default_resource example
#include <fstream>
#define json_out

#include <boost/algorithm/string.hpp>

using namespace std;
using namespace SimpleWeb;
//Added for the json-example:
using namespace boost::property_tree;
void my_handler(int s) {
	printf("Caught signal %d\n", s);
	exit(1);
}

void signalHandler(int signum)
{
	cout << "Interrupt signal (" << signum << ") received.\n";

	// cleanup and close up stuff here  
	// terminate program  

	exit(signum);

}
int main() {
	signal(SIGINT, signalHandler);

	//HTTP-server at port 8080 using 4 threads
	std::string ip = "127.10.1.1";
	unsigned short port = 80;
	unsigned short ssl_port = 443;
	try {
		Server<HTTP> server(ip, port, 4);
		Server<HTTPS> server_ssl(ip, ssl_port, 1, "127.10.1.1.crt", "127.10.1.1.key", 5, 300, "Forseta_CA.crt");


#pragma region Set Server.resource
		server.resource["^/$"]["GET"] = [&](ostream& response, shared_ptr<Request> request) {
			std::string out = "test is OK";
			response << "HTTP/1.1 200 OK\r\nContent-Length: " << out.length() << "\r\n\r\n" << out;
		};


#pragma endregion    

		server_ssl.resource = server.resource;
		server_ssl.default_resource = server.default_resource;
		thread server_thread([&server]() {
			//Start server
			server.start();
		});
		thread server_thread_ssl([&server_ssl]() {
			//Start server
			server_ssl.start();
		});
		//Wait for server to start so that the client can connect
		this_thread::sleep_for(chrono::seconds(1));


		std::cout << "Server started address: http://" << ip << ":" << port << " or https://" << ip << ":" << ssl_port << "\n";

		//server_thread.join();
		//server_thread_ssl.join();
		std::cout << "Press n to stop";
		while (true)
		{
			
			if (cin.get() == 'n') 
				break;
		};
		server.stop();
		server_ssl.stop();
		std::cout << "Stoping";
		server_thread.join();
		server_thread_ssl.join();
	
	}
	catch (exception& e) {
		std::cout << "Server error"<< e.what() << "\n";
		
		
		return 2;
	}
	return 0;
}