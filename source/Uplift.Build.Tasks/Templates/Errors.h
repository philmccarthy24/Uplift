#pragma once

#include <string>

namespace Uplift
{
	namespace Core
	{
		class SoapProcessingException : public std::exception
		{
		public:
			SoapProcessingException(const std::string& message) :
				std::exception(message.c_str())
			{
			}
			virtual ~SoapProcessingException()
			{
			}
		};
	}

	namespace Isapi
	{
		class BadRequestException : public std::exception
		{
		public:
			BadRequestException(const std::string& message) :
				std::exception(message.c_str())
			{
			}
			virtual ~BadRequestException()
			{
			}
		};

		class InternalServerErrorException : public std::exception
		{
		public:
			InternalServerErrorException(const std::string& message) :
				std::exception(message.c_str())
			{
			}
			virtual ~InternalServerErrorException()
			{
			}
		};

		class MethodNotAllowedException : public std::exception
		{
		public:
			MethodNotAllowedException(const std::string& message) :
				std::exception(message.c_str())
			{
			}
			virtual ~MethodNotAllowedException()
			{
			}
		};

		class InvalidMediaTypeException : public std::exception
		{
		public:
			InvalidMediaTypeException(const std::string& message) :
				std::exception(message.c_str())
			{
			}
			virtual ~InvalidMediaTypeException()
			{
			}
		};

		class NotFoundException : public std::exception
		{
		public:
			NotFoundException(const std::string& message) :
				std::exception(message.c_str())
			{
			}
			virtual ~NotFoundException()
			{
			}
		};
	}
}