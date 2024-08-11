#include <stdio.h>
#include <string.h>
#include <stdbool.h>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <time.h>
#include <unistd.h>

int main(void) {
	int socket_desc;
	struct sockaddr_in server_addr, client_addr;
	char server_message[26], client_message[26];
	
	int server_struct_length = sizeof(server_addr);
	int client_struct_length = sizeof(client_addr);

	socket_desc = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);

	if(socket_desc < 0) {
		printf("Error while creating socket\n");
	}
	printf("Socket created successfully\n");

	server_addr.sin_family = AF_INET;
	server_addr.sin_port = htons(5144);
	server_addr.sin_addr.s_addr = inet_addr("127.0.0.1");

	while (true) {
		memset(server_message, '\0', server_struct_length);
		memset(client_message, '\0', client_struct_length);

		time_t timer;
		struct tm* tm_info;
		timer = time(NULL);
		tm_info = localtime(&timer);
		strftime(client_message, 26, "%Y-%m-%d %H:%M:%S", tm_info);

		if(sendto(socket_desc, client_message, strlen(client_message), 0, (struct sockaddr*)&server_addr, server_struct_length) < 0) {
			printf("Unable to send message\n");
			return -1;
		}
		
		if(recvfrom(socket_desc, server_message, sizeof(server_message), 0, (struct sockaddr*)&server_addr, &server_struct_length) < 0) {
			printf("Error while receiving server's message\n");
			return -1;
		}

		printf("Server's response: %s\n", server_message);

		sleep(1);
	}

	close(socket_desc);

	return 0;
}
