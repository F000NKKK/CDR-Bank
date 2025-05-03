# replace params
envsubst "$(printf '${%s} ' $(env | cut -d'=' -f1))" < ./appsettings.json | sponge ./appsettings.json

# start app
dotnet ./VP.WTrack.ApiGateway.Server.dll --environment=$ASPNETCORE_ENVIRONMENT