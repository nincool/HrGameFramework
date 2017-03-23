#ifndef _HR_SIGNAL_H_
#define _HR_SIGNAL_H_

#include "HrFramework/Interface/IDispatcher.h"
#include <functional>
#include <tuple>
#include <boost/any.hpp>
#include <boost/assert.hpp>

namespace Hr
{
	class HrSignal : public IDispatcher
	{
	public:
		HrSignal();
		virtual ~HrSignal();

		virtual void Register(const Invoker& invoker, void* pTarget, const std::string& strKey) override;
		virtual void Unregister(void* pTarget, const std::string& strKey);
		virtual void Unregister(void* pTarget);
		virtual void Dispatch(const HrParamPtr& pParam) override;

	protected:
		std::unordered_map<size_t, std::list< std::pair<size_t, Invoker> > > m_mapInvokers;
	};
}

#endif


