#ifndef SERVER_H
#define	SERVER_H	

#include "server_http.hpp"
#include "server_https.hpp"
using namespace SimpleWeb;
namespace BobsHttpWeb {
	typedef boost::asio::ip::tcp::socket HTTPWhitSSL;
	template<>
	class Server<HTTPWhitSSL> : public ServerBase<HTTPWhitSSL>{
	public:
		Server(unsigned short port, size_t num_threads = 1, size_t timeout_request = 5, size_t timeout_content = 300) :
			ServerBase<HTTP>::ServerBase(port, num_threads, timeout_request, timeout_content) {};

	private:
		void accept() {
			//Create new socket for this connection
			//Shared_ptr is used to pass temporary objects to the asynchronous functions
			std::shared_ptr<HTTPWhitSSL> socket(new HTTP(m_io_service));

			acceptor.async_accept(*socket, [this, socket](const boost::system::error_code& ec) {
				//Immediately start accepting a new connection
				accept();

				if (!ec) {
					read_request_and_content(socket);
				}
			});
		}
	};
}

#endif	/* SERVER_H */