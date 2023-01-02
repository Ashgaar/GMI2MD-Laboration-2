# file: subscribe_hajo66_mqtt_v3_advanced.py
import sys
import datetime
import base64
import json
import paho.mqtt.client as mqtt

THE_BROKER = "eu1.cloud.thethings.network"
#MY_TOPICS = 'v3/+/devices/+/up'
MY_TOPICS = 'v3/miol00-test-device@ttn/devices/eui-70b3d57ed005803e/up'
APP_ID = 'miol00-test-device@ttn'
API_KEY = 'NNSXS.4UC7MI7DRMQOXTFJJOCO42M7UV4ZCUGVAVGO6ZY.O7LMGTORRNONL4EBPAT7OS7MWZ2WRDRZQTN5D6PQEBJEO6J6HUNQ'
# THE_BROKER = "eu1.cloud.thethings.network"
# MY_TOPICS = 'v3/+/devices/+/up'
# APP_ID = 'gmi2md-application-1'
# API_KEY = "NNSXS.IMPCNYX6PIVO4ZGRIXVWMDRGBQIZKCUJU657XMA.GOI4KVEVDIEDPSVSI7DLUKQER7NLGL2O5FE23DJIE2ENKTOHKPWA"

def format_time():
    tm = datetime.datetime.now()
    sf = tm.strftime('%Y-%m-%d %H:%M:%S.%f')
    return sf[:-4]

def format_message(themsg):

    message = json.loads(themsg.decode('utf-8'))
    
    base64_bytes = message["uplink_message"]["frm_payload"].encode('ascii')
    message_bytes = base64.b64decode(base64_bytes)
    cleanMessage = message_bytes.decode('ascii')
    
    print(f'Pretty output: {MY_TOPICS}\ndevice_id: {message["end_device_ids"]["device_id"]}\nappilication_id: {message["end_device_ids"]["application_ids"]["application_id"]}\nfrm_payload: {message["uplink_message"]["frm_payload"]}\nfrm_payload_b64decode: {cleanMessage}')
    
    

def on_connect(client, userdata, flags, rc):
    print(f'[+] Connected to: {client._host}, port: {client._port}')
    print(f'Flags: {flags},  return code: {rc}')
    client.subscribe(MY_TOPICS, qos=0)        # Subscribe to all topics

def on_subscribe(mosq, obj, mid, granted_qos):
    print(f'Subscribed to topics: {MY_TOPICS}')
    print('Waiting for messages...')

def on_message(client, userdata, msg):
    themsg = str(msg.payload)
    print(f'\nReceived topic: {str(msg.topic)} with payload: {themsg}, at subscribers local time: {format_time()}')
    format_message(msg.payload)

def on_disconnect(client, userdata, rc):
    print("disconnect")
    client.disconnect()

client = mqtt.Client()

client.on_connect = on_connect
client.on_message = on_message
client.on_subscribe = on_subscribe
client.on_disconnect = on_disconnect

print(f'Connecting to TTN V3: {THE_BROKER}')
# Setup authentication from settings above
client.username_pw_set(APP_ID, API_KEY)

try:
    # IMPORTANT - this enables the encryption of messages
    client.tls_set()	# default certification authority of the local system
    client.connect(THE_BROKER, port=8883, keepalive=60)

except BaseException as ex:
    print(f'Cannot connect to TTN V3: {THE_BROKER}')
    print(f"TTN V3 error: {ex}")
    sys.exit()

client.loop_forever()
