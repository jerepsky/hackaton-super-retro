docker build -t hackathon-super-retro .

docker run -d -p 5295:80 --name super-retro-be hackathon-super-retro
