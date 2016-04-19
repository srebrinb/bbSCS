netsh http delete urlacl http://127.0.0.1:53951/
netsh http delete urlacl https://127.0.0.1:53952/
netsh http delete sslcert ipport=127.0.0.1:53952
netsh http delete urlacl http://localhost:53951/
netsh http delete urlacl https://localhost:53952/
netsh http delete sslcert ipport=localhost:53952
