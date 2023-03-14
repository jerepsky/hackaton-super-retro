docker build -t hackaton-super-retro .

docker run -d -p 5295:80 --name super-retro-be hackaton-super-retro
