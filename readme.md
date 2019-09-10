# Code Challenge — Authorizer

## Stack
* .NET Core
* C Sharp
* Docker

# Build and run

## Install Docker

### Mac

 1. [Download Docker](https://download.docker.com/mac/beta/Docker.dmg)
 2. Double-click the DMG file, and drag-and-drop Docker into your
    Applications folder.
 3. You need to authorize the installation with your system password.
 4. Double-click Docker.app to start Docker.
 5. The whale in your status bar indicates Docker is running and
    accessible.
 6. Docker presents some information on completing common tasks and
    links to the documentation.
 7. You can access settings and other options from the whale in the
    status bar. a. Select About Docker to make sure you have the latest
    version.

### Linux
If you use Ubuntu Trusty, Wily, or Xenial, install the linux-image-extra kernel package:

> sudo apt-get update -y && sudo apt-get install -y
> linux-image-extra-$(uname -r)

Install Docker: 

> sudo apt-get install docker-engine -y

Start Docker: 

>  sudo service docker start

Verify Docker:

>  sudo docker run hello-world

