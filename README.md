# LightningConstructs
A small library full of extremly fast and useful tidbits.

Library contents:

 + QueueStream: A high speed, high efficency generic queue, which enqueues and dequeues like a stream using arrays.
 + Governor: A highly accurate loop governor, with ~200 microsecond precision.
 + Socket: A thread safe TCP socket, with accurate disconnect detection.
 + Dictionary: A value-refrence based generic dictionary, built for high speed access and storage.
 + SpinGate: A generic blocking-wait style synchronization class, with the ability to return data to the locking thread. Based off of Governor.
 + AsyncConsole: A utility class for writing to the console/terminal asynchronously.