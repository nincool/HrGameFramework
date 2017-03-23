#ifndef _HR_PARAM_H_
#define _HR_PARAM_H_

#include "HrFramework/HrFrameworkPrerequisite.h"
#include "HrFramework/HrFrameUtil.h"

namespace Hr
{
	class HrParam
	{
	public:
		template <typename T>
		T GetData(int nIndex)
		{
			BOOST_ASSERT(0 <= nIndex && nIndex < m_lisDatas.size());
			return HrBoostAnyCast(m_lisDatas[nIndex]);
		}

		template <typename T>
		void AddData(T& t)
		{
			m_lisDatas.push_back(boost::any(t));
		}

	protected:
		template <typename T>
		void Unpack(HrParam& param, T&& t)
		{

		}

		template <typename... Args>
		void MakeParamHelper(HrParam& param, Args&&... args)
		{
			int dummy[] = { 0 , (Unpack(param, std::forward<Args>(args)), 0)... };
		}

		template <typename... Args>
		void MakeParam(HrParam& param, Args&&... args)
		{
			MakeParamHelper(param, std::forward(args)...)
		}
	public:
		std::vector<boost::any> m_lisDatas;
	};
}


#endif


