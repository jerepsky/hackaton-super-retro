docker build -t hackaton-super-retro .

docker run -d -p 5295:80 --name super-retro-be hackaton-super-retro

docker export --output="super-retro-be.tar" super-retro-be

docker import super-retro-be.tar hackaton-super-retro
