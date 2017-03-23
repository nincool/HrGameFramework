#ifndef _HR_CONTEXT_H_
#define _HR_CONTEXT_H_

#include "HrFramework/HrFrameworkPrerequisite.h"

namespace Hr
{
	class HrContext
	{
	public:
		HrContext();
		virtual ~HrContext();

		template <typename T>
		void BindCommand()
		{
			if (typeid(T) == typeid(HrSignalCommandBinder))
			{
				if (m_pSignalCommandBinder)
				{
					m_pSignalCommandBinder.reset();
				}
				m_pSignalCommandBinder = std::make_shared<T>();
			}
			else if (typeid(T) == typeid(HrEventCommandBinder))
			{
				if (m_pEventCommandBinder)
				{
					m_pEventCommandBinder.reset();
				}
				m_pEventCommandBinder = std::make_shared<T>();
			}
		}
		template <typename T>
		void BindMediator()
		{
			if (m_pMediationBinder)
			{
				m_pMediationBinder.reset();
			}
			m_pMediationBinder = std::make_shared<T>();
		}
	//protected:
		virtual void MapBindings();
	protected:
		HrCommandBinderPtr m_pEventCommandBinder;
		HrCommandBinderPtr m_pSignalCommandBinder;
		HrMediationBinderPtr m_pMediationBinder;


	};
}


#endif


