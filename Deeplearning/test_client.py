import zmq
import time
import sys

port = "5555"

context = zmq.Context()
print("Connecting to server...")
socket = context.socket(zmq.REQ)
socket.connect("tcp://localhost:%s" % port)

    
for request in range (1,10):
    # Send answer
    answer = input("\n->")
    socket.send_string(answer)
    start_time = time.time()
    print(f"Sent request: {answer}")
    
    
    #  Get the reply.
    question = socket.recv()
    end_time = time.time()
    question = question.decode("utf-8")
    print(f"Received reply: {question} ---- in {(end_time - start_time):.2f}sec")

