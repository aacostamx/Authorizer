
# Code Challenge — Authorizer

## Stack
* C#
* .NET Core
* Unit testing
* Docker

![Stack](https://lh3.googleusercontent.com/8zfYvZSgJ48ytE-iBLYqJHzDKjgaA1_HBQn378TYh6DuGIyn-FbdfzDaSTzatqUoRdLWlSQEIrPv)

## Build and run

### Install Docker
[https://docs.docker.com/install/](https://docs.docker.com/install/)

### Download Docker Image 
WeTransfer:
https://we.tl/t-Gk3d51w4B6

Dropbox:
https://www.dropbox.com/s/oneip7569cwywwt/authorizetransaction.tar?dl=0

**Image Name:**
> **authorizetransaction.tar**
> 180MB (for the Net Core Runtime)

![Docker Image](https://lh3.googleusercontent.com/JkQ86hKvOUqWmB8XcIFVRwbuyTJbk-yv9cK5SEc9I557WC7a5TGWMpKMK3-QWvRVTdfSKdyamkEX)

### Install Docker Image
Open Bash 
Download Ubuntu Image (only for Windows)

    docker pull ubuntu

Navigate to the Image path
CDM example:

> cd c:\

**Load Docker Image**

> docker load --input authorizetransaction.tar

Check Images
> `docker images`

*Must be an authorizetransaction image*

Run image

> docker run -i authorizetransaction ubuntu bash

Enter the JSON example
> { "account": { "activeCard": true, "availableLimit": 100 } } 
> {"transaction": { "merchant": "Burger King", "amount": 20, "time": "2019-02-13T10:00:00.000Z" } } 
> { "transaction": { "merchant":"Habbib's", "amount": 60, "time": "2019-02-13T11:00:00.000Z" } }
> 
**Hit Enter to start analyzing**
Get results

![CDM example](https://lh3.googleusercontent.com/NyjZmJby7zscOBCYCXOz19pqKcb23wmae7VadkykQx70VkIda2ZoRVD6xYiA4ZQElnQ2360-giC_)
