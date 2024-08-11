use redis::{Client, Commands, RedisError};

pub fn send_to_redis(data: &[u8]) -> Result<(), RedisError> {
    let client = Client::open("redis://127.0.0.1/")?;
    let mut con = client.get_connection()?;
    let _ = con.set("timestamp", data)?;
    Ok(())
}