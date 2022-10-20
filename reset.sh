# git clean -fxd
# git reset --hard

dotnet ef database drop --project VendingMachine.EF -f && dotnet ef migrations remove --project VendingMachine.EF -f && dotnet ef migrations add InitialCreate --project VendingMachine.EF && dotnet ef database update --project VendingMachine.EF
