#ifndef _HR_FRAMEWORKPREREQUISITE_H_
#define _HR_FRAMEWORKPREREQUISITE_H_

#include <memory>
#include <typeinfo>       // operator typeid
#include <typeindex>      // std::type_index
#include <unordered_map>
#include <functional>

namespace Hr
{

	class HrFrameRoot;
	using HrFrameRootPtr = std::shared_ptr<HrFrameRoot>;

	class IListener;
	using IListenerPtr = std::shared_ptr<IListener>;
	class IDispatcher;
	using IDispatcherPtr = std::shared_ptr<IDispatcher>;

	class ITrigger;

	class HrContext;
	using HrContextPtr = std::shared_ptr<HrContext>;
	
	class HrBinder;
	using HrBinderPtr = std::shared_ptr<HrBinder>;
	class HrCommandBinder;
	using HrCommandBinderPtr = std::shared_ptr<HrCommandBinder>;
	class HrEventCommandBinder;
	using HrEventCommandBinderPtr = std::shared_ptr<HrEventCommandBinder>;
	class HrSignalCommandBinder;
	using HrSignalCommandBinderPtr = std::shared_ptr<HrSignalCommandBinder>;
	class HrMediationBinder;
	using HrMediationBinderPtr = std::shared_ptr<HrMediationBinder>;

	class HrCommand;
	using HrCommandPtr = std::shared_ptr<HrCommand>;

	class HrSignal;
	using HrSignalPtr = std::shared_ptr<HrSignal>;

	class HrParam;
	using HrParamPtr = std::shared_ptr<HrParam>;

	class HrView;

	using Invoker = std::function<void(const HrParamPtr& pParam)>;

}

#define HR_CALLBACK_0(__selector__,__target__, ...) std::bind(&__selector__,__target__, ##__VA_ARGS__)
#define HR_CALLBACK_1(__selector__,__target__, ...) std::bind(&__selector__,__target__, std::placeholders::_1, ##__VA_ARGS__)
#define HR_CALLBACK_2(__selector__,__target__, ...) std::bind(&__selector__,__target__, std::placeholders::_1, std::placeholders::_2, ##__VA_ARGS__)
#define HR_CALLBACK_3(__selector__,__target__, ...) std::bind(&__selector__,__target__, std::placeholders::_1, std::placeholders::_2, std::placeholders::_3, ##__VA_ARGS__)


#endif



