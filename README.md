# EasyWeChat
模仿微信

安装docker

```
两步解决yum无法安装软件问题：Cannot find a valid baseurl for repo: centos-sclo-rh/x86_64

1、进入/etc/yum.repos.d目录，删除目录下面所有的软件包，删除命令如下：
sudo rm -rf *

2、下载安装阿里云镜像仓库：
sudo curl -o /etc/yum.repos.d/CentOS-Base.repo https://mirrors.aliyun.com/repo/Centos-7.repo

3、下载安装阿里云epel.repo：
sudo curl -o /etc/yum.repos.d/epel.repo https://mirrors.aliyun.com/repo/epel-7.repo

4、清除缓存：
yum clean all

5、生成缓存：
yum makecache
yum makecache fast
```



## 数据库mysql

```dockerfile
docker pull mysql:5.7
mkdir ~/mysql
cd ~/mysql
docker run -d -p 3306:3306 --name chen_mysql -v $PWD/conf:/etc/mysql/conf.d -v $PWD/logs:/logs -v $PWD/data:/var/lib/mysql -e MYSQL_ROOT_PASSWORD=chen mysql:5.7
```

- -p 3306:3306：将主机的3306端口映射到docker容器的3306端口。
- --name chen_mysql：运行服务名字
- -v $PWD/conf:/etc/mysql/conf.d ：将主机/root/mysql录下的conf/my.cnf 挂载到容器的  /etc/mysql/conf.d 
- -v $PWD/logs:/logs：将主机/root/mysql目录下的 logs 目录挂载到容器的 /logs。
- -v $PWD/data:/var/lib/mysql ：将主机/root/mysql目录下的data目录挂载到容器的 /var/lib/mysql  
- -e MYSQL_ROOT_PASSWORD=chen：初始化 root 用户的密码。
- mysql:5.7: 后台程序运行mysql5.7

````
运行以下命令可进入mysql命令窗口：
	 docker exec -it c_mysql /bin/bash
登录mysql:
	 mysql -uroot -chen
````

## Redis

```dockerfile
docker pull redis:7.0.5
mkdir ~/redis
cd ~/redis
docker run -d -p 6379:6379 --name chen_redis --privileged=true -v $PWD/conf/redis.conf:/etc/redis/redis.conf -v $PWD/data:/data redis:7.0.5 
redis-server /etc/redis/redis.conf --appendonly yes
```

- --privileged=true：表示给容器root权限，否则无法使用appenonly
- redis-server：表示使用右侧路径中的配置文件启动。
- --appendonly yes : 指定以aof形式启动

```
wget https://mirrors.aliyun.com/docker-ce/linux/centos/docker-ce.repo -O /etc/yum.repos.d/docker-ce.repo
```

