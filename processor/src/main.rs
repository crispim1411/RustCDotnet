use std::{error::Error, net::UdpSocket, sync::mpsc, thread::{self, JoinHandle}};

mod redis_connector;

fn main() -> Result<(), Box<dyn Error>> {
    let socket = UdpSocket::bind("127.0.0.1:5144")?;

    let (tx, rx) = mpsc::channel();

    let generator_handler = thread::spawn(move || {
        loop {
            let mut buf = [0; 26];
            let Ok((_, src)) = socket.recv_from(&mut buf) else { continue; };
            // send to redis
            let Ok(()) = tx.send(buf) else { continue; };
            // // send back
            let Ok(_) = socket.send_to(&buf, &src) else { continue; };
        }
    });

    let redis_handler = thread::spawn(move || {
        loop {
            if let Ok(data) = rx.recv() {
                println!("Sending data length: {}", data.len());
                let Ok(_) = redis_connector::send_to_redis(&data) else { continue };
            }
        }
    });
    
    generator_handler.join().expect("Generator thread has panicked");
    redis_handler.join().expect("Redis thread has panicked");

    Ok(())
}
