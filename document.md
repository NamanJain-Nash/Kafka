# Kafka + .Net + Docker

## Setup Kafka
<p>It's very simple have a docker compose that we have need to run to start the kafka and it to be done and then to do so we can start both the zookeper and the kafka container</p>

<p>We will start using 'docker compose up -d'[To make it run in detach mode.]</p>

## Setting up the Topic

<p>In kafka their are various worker node with each havinhg topics where it is stored</p>

### Get The Docker Access Connection to start the enter kafka terminal
<p>docker exec -it kafka /bin/bash </p>

### Getting The topic Ready

<p>kafka-topics.sh \
--create \
--topic test\
--partitions 1 \
--replication-factor 1 \
--if-not-exists \
--zookeeper zookeeper:2181</p>

### Check the Topics and etc
<p>kafka-topics.sh \
--list \
--zookeeper zookeeper:2181

We can also check the listed topic details also

kafka-topics.sh \
--describe \
--topic test \
--zookeeper zookeeper:2181
</p>

### Publish and Subscribe using a Terminal
<p>kafka-console-producer.sh \
--request-required-acks 1 \
--broker-list 0.0.0.0:9092 \
--topic test

We will be geting the rpoducer sh file and then request its Producer api and then use that to send using its ip and also the topic where it should be done
</p>

#### Making Read of the Topic check / consume it
<p>
kafka-console-consumer.sh \
--bootstrap-server 0.0.0.0:9092 \
--topic foo \
--zookeeper  zookeeper:2181
</p>

## Producer API in .Net

<p></p>




