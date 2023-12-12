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

## .Net Application

<p>The application is Designed in such a way that we have a Producer and a consumer in the case to get full utlization of the KafkA is been done.

The main scope of this application is to have a way to send the mesage and recive it and we have made a api to easily post the message and have also make a console application to get the consumer be their.

The pattern followed in a way that every unit is designed so reuability of the code is high </p>

### Api Application
<p>The main aim is that we have a controller where a post call is their to send the topic and the data be their and then use it to send the main buisness layer where conversion of data is their to provide mapping and other logic check</p>

### Buisness Layer
<p>The application is such that we have to convert the DTO to the main data and when we see it uses the producer service to post to the Service layer.</p>

### Service Layer
<p>We will see their is a class where we use the consumer of kafka to encode the data and send it to the required topic and then be used inside the kafka and that make it be utilized by the consumer</p>

### Kafka
<p>It will Store and manage it using the Zookepper and be used in the Subscriber and that can be many</p>

### Consumer
<p> We will see that this is the consume the application that it uses the service to easily see and have a connection that is their to recive the connection and according to the logic that we have to open and close a logic and then utilized to see the console application that is their.</p>

### Service Layer
<p>The main application that is their which is used to easily open and close the connection to collect the data from the respective group and its respective topic that is their and make the use of it to easily send into that group.</p>

### Kafka memory
<p>It make the store of the memory locally</p>

## Kafka Application

### Get the calls

<p> We have setb the whole process to get the details and have used the api to publish so before start consumer we can get the info using

To get the amount of pushed content
docker exec -it kafka /opt/kafka/bin/kafka-con
kafka-console-consumer.sh --topic test-topic --from-beginning --bootstrap-server localhost:9092
</p>

<p>Afte this we can easily see the topic and the work to get the whole info and the work be done</p>

<p></p>