#include "HrFramework/Signal/HrSignal.h"

using namespace Hr;

HrSignal::HrSignal()
{
}

HrSignal::~HrSignal()
{
}

void HrSignal::Register(const Invoker& invoker, void* pTarget, const std::string& strKey)
{
	size_t nHashTargetKey = reinterpret_cast<size_t>(pTarget);
	size_t nHashInvokerKey = std::hash_value(strKey);
	auto iteTargetInvokers = m_mapInvokers.find(nHashTargetKey);
	if (iteTargetInvokers != m_mapInvokers.end())
	{
		for (auto iteInvoer : iteTargetInvokers->second)
		{
			if (iteInvoer.first == nHashInvokerKey)
			{
				BOOST_ASSERT(nullptr);
				return;
			}
		}
		iteTargetInvokers->second.push_back(std::make_pair(nHashInvokerKey, invoker));
	}
	else
	{
		std::list< std::pair<size_t, Invoker> > lisInvokers;
		lisInvokers.push_back(std::make_pair(nHashInvokerKey, invoker));
		m_mapInvokers[nHashTargetKey] = lisInvokers;
	}
}

void HrSignal::Unregister(void* pTarget, const std::string& strKey)
{
	size_t nHashTargetKey = reinterpret_cast<size_t>(pTarget);
	size_t nHashInvokerKey = std::hash_value(strKey);
	auto iteTargetInvokers = m_mapInvokers.find(nHashTargetKey);
	if (iteTargetInvokers != m_mapInvokers.end())
	{
		iteTargetInvokers->second.erase(std::find_if(iteTargetInvokers->second.begin(), iteTargetInvokers->second.end(), [&](std::pair<size_t, Invoker>& item) {
			return item.first == nHashInvokerKey;
		}));
	}
}

void HrSignal::Unregister(void* pTarget)
{
	size_t nHashTargetKey = reinterpret_cast<size_t>(pTarget);
	auto iteTargetInvokers = m_mapInvokers.find(nHashTargetKey);
	if (iteTargetInvokers != m_mapInvokers.end())
	{
		m_mapInvokers.erase(iteTargetInvokers);
	}
}

void HrSignal::Dispatch(const HrParamPtr& pParam)
{
	for (auto iteTargetInvokers : m_mapInvokers)
	{
		for (auto iteInvoker : iteTargetInvokers.second)
		{
			(iteInvoker.second)(pParam);
		}
	}
}

