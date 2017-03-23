#ifndef _I_DISPATCHER_H_
#define _I_DISPATCHER_H_

#include "HrFramework/HrFrameworkPrerequisite.h"

namespace Hr
{
	class IDispatcher
	{
	public:
		virtual ~IDispatcher() {};

		virtual void Register(const Invoker& invoker, void* pTarget, const std::string& strKey) = 0;
		
		virtual void Unregister(void* pTarget, const std::string& strKey) = 0;
		
		virtual void Unregister(void* pTarget) = 0;
		
		virtual void Dispatch(const HrParamPtr& pParam) = 0;
	
	};
}

#endif


