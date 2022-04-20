tdd:
	cd test && dotnet watch test --logger "console;verbosity=detailed" && cd ..

test-run:
	cd test && dotnet test --logger "console;verbosity=detailed" && cd ..

ssl: 
	cd src && dotnet dev-certs https --trust && cd ..